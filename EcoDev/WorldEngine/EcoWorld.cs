using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common;
using EcoDev.Engine.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using EcoDev.Core.Common.Maps;
using EcoDev.Core.Common.Actions;
using EcoDev.Core.Common.BuildingBlocks;

namespace EcoDev.Engine.WorldEngine
{
	public class EcoWorld : IWorld, IDisposable
	{
		Map _worldMap;
		CancellationTokenSource _tokenSource;
		List<LivingEntityWithQualities> _inhabitants = new List<LivingEntityWithQualities>();
		string _worldName;
		Task _worldTask = null;
		const int MIN_MILLISECONDS_TO_CYCLE_THROUGH_PLAYER_ACTIONS = 1000;
		public InhabitantPositionEngine _positionEngine = new InhabitantPositionEngine();
		static object _debugLock = new object();
		bool _enableDebug = false;

		public event EventHandler<DebugInfoEventArgs> DebugInformation;
		public event EventHandler<EntityExitEventArgs> EntityExited;
		public event EventHandler<InhabitantActionEventArgs> InhabitantPerformedAction;

		public EcoWorld(string worldName, Map worldMap, LivingEntityWithQualities[] inhabitants, bool enableDebug)
		{
			_worldName = worldName;
			_enableDebug = enableDebug;

			if (worldMap == null)
			{
				throw new ArgumentException("World Map cannot be NULL");
			}
			_worldMap = worldMap;
			_worldMap.InitialiseMap();

			if (inhabitants != null && inhabitants.Length > 0)
			{
				_inhabitants.AddRange(inhabitants);
			}

			_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken(false));
		}

		public Map WorldMap { get { return _worldMap; } }
		public IEnumerable<LivingEntityWithQualities> Inhabitants { get { return _inhabitants; } }

		public void AddPlayer(LivingEntityWithQualities player)
		{
			_inhabitants.Add(player);
			AddInhabitantsToMap();

		}

		protected void FireDebugInfoEvent(string debugInfo)
		{
			if (DebugInformation != null && _enableDebug)
			{
				lock (_debugLock)
				{
					DebugInformation(this, new DebugInfoEventArgs(FormatDebugInformation(debugInfo)));
				}
			}
		}

		protected void FireInhabitantPerformedActionEvent(ActionToPerform actionPerformed, MovementDirection directionMoved, MapPosition positionInMap)
		{
			if (InhabitantPerformedAction != null)
			{
				Task.Factory.StartNew(() =>
				{
					InhabitantPerformedAction(this, new InhabitantActionEventArgs(actionPerformed, directionMoved, positionInMap));
				});
			}
		}

		private string FormatDebugInformation(string debugInfo)
		{
			string fullInfo = string.Format("[{0} - {1}] {2}{3}", DateTime.Now.ToShortDateString(), DateTime.Now.ToString("hh:mm:ss"), debugInfo, Environment.NewLine);
			return fullInfo;
		}

		protected void FireEntityExitedEvent(LivingEntityWithQualities entity)
		{
			if (EntityExited != null)
			{
				EntityExited(this, new EntityExitEventArgs(entity));
			}
		}

		private void FireDebugInfoEvent(string debugInfo, params object[] args)
		{
			FireDebugInfoEvent(string.Format(debugInfo, args));
		}

		public void StartWorld()
		{
			FireDebugInfoEvent("{0}************{0}World [{1}]{0}************{0}Starting World: '{1}' processing loop", Environment.NewLine, _worldName);

			_worldTask = Task.Factory.StartNew(new Action(WorldProcessingTask), _tokenSource.Token);
		}

		public void DestroyWorld()
		{
			DebugInformation = null;

			Stopwatch cancelTimer = new Stopwatch();
			_tokenSource.Cancel();
			while (!_worldTask.IsCompleted && cancelTimer.ElapsedMilliseconds < 2000)
			{
			}

			try
			{
				_worldTask.Dispose();
			}
			catch { }
		}

		internal void WorldProcessingTask()
		{
			FireDebugInfoEvent("Starting World Processing: [{0}]", _worldName);

			while (!_tokenSource.IsCancellationRequested)
			{
				ProcessPlayers();

				CleanupWorld();
			}

		}

		private void CleanupWorld()
		{
			//TODO: Remove any dead players from world.
			//TODO: Also remove any players who have found an exit.
			//Note: Players who have found an exit will need to be attributed accordingly.
			var deadPlayers = _inhabitants.Where(i => i.IsDead).ToList();
			if (deadPlayers.Count > 0)
			{
				deadPlayers.ForEach(d =>
					{
						FireDebugInfoEvent("Removing Player [{0}] from World.", d.Entity.Name);
						_inhabitants.Remove(d);
					});
			}

			List<LivingEntityWithQualities> playersCompleted = new List<LivingEntityWithQualities>();
			for (int cnt = 0; cnt < _inhabitants.Count; cnt++)
			{
				var inhabitant = _inhabitants[cnt];
				var posCtxt = _positionEngine.ConstructPositionContextForEntity(inhabitant, _worldMap);
				if (posCtxt.CurrentPosition is MapExitBlock)
				{
					playersCompleted.Add(inhabitant);
				}

			}
			playersCompleted.ForEach(p =>
			{
				_inhabitants.Remove(p);
				FireEntityExitedEvent(p);
			});

		}

		internal void ProcessPlayers()
		{
			var timer = new Stopwatch();
			timer.Start();
			for (int loop = 0; loop < _inhabitants.Count; loop++)
			{
				var entity = _inhabitants[loop];
				PerformEntityAction(entity);
			}

			while (timer.ElapsedMilliseconds < MIN_MILLISECONDS_TO_CYCLE_THROUGH_PLAYER_ACTIONS) { }
		}

		internal void AddInhabitantsToMap()
		{
			FireDebugInfoEvent("Adding inhabitants to map");
			int addedCount = 0;

			if (_inhabitants != null && _inhabitants.Count > 0)
			{
				for (var cnt = 0; cnt < _inhabitants.Count; cnt++)
				{
					var entity = _inhabitants[cnt];
					if (entity.Entity.World == null)
					{
						entity.Entity.World = this;
						var entrance = FindAnEntrance();
						entity.PositionInMap = entrance;
						entity.ForwardFacingAxis = entrance.DetermineForwardFacingPositionBasedOnThisPosition(_worldMap.WidthInUnits, _worldMap.HeightInUnits, _worldMap.DepthInUnits);
						addedCount++;
					}
				}
			}
			FireDebugInfoEvent("Added {0} new inhabitants to world", addedCount);
		}


		private void PerformEntityAction(LivingEntityWithQualities entity)
		{
			if (entity.IsDead)
			{
				return;
			}

			FireDebugInfoEvent("Performing entity action on Entity:{0}", entity.Entity.Name);
			var positionContext = _positionEngine.ConstructPositionContextForEntity(entity, _worldMap);
			ActionContext context = new ActionContext(positionContext);

			var asyncEngine = new AsyncActionExecutionEngine(entity, context);
			var result = asyncEngine.DetermineEntityAction();

			if (result.ErrorException == null)
			{
				ActOnEntityActionResult(entity, result.ActionResult);
				FireInhabitantPerformedActionEvent(result.ActionResult.DecidedAction, result.ActionResult.DirectionToMove, entity.PositionInMap.Clone());
			}
			else
			{
				//TODO: Mark entity as invalid and remove in garbage collection phase
				FireDebugInfoEvent("Player {0} threw exception. Marking as invalid and removing from world.", entity.Entity.Name);
			}
		}

		private void ActOnEntityActionResult(LivingEntityWithQualities entity, ActionResult actionResult)
		{
			FireDebugInfoEvent("Acting on Entity:{0} Result, ActionResult:{1}", entity.Entity.Name, actionResult.DecidedAction);
			var responseActionHandler = ActionResponseFactory.CreateActionResponseHandler(actionResult, entity, this);
			try
			{
				responseActionHandler.ExecuteActionToPerform();
				var positionCOntext = _positionEngine.ConstructPositionContextForEntity(entity, _worldMap);
				if (positionCOntext.CurrentPosition is MapExitBlock)
				{
					// Player has found an exit.
					FireDebugInfoEvent("Inhabitant [{0}] has found an exit! Now exiting world.", entity.Entity.Name);
				}
			}
			catch (Exception ex)
			{
				HandlePlayerCriticalError(entity);
			}
			//throw new NotImplementedException();
		}

		private void HandlePlayerCriticalError(LivingEntityWithQualities entity)
		{
			//TODO: May need to remove player from the world
			throw new NotImplementedException();
		}


		internal MapPosition FindAnEntrance()
		{
			if (_worldMap.Entrances.Count() == 0)
			{
				throw new MapInvalidException("No entrances available in world or world not initialised");
			}

			if (_worldMap.Entrances.Count() == 1)
			{
				return _worldMap.Entrances.First();
			}
			else
			{
				var allEntrances = _worldMap.Entrances.ToList();
				Random rnd = new Random(DateTime.Now.Millisecond);
				return allEntrances[rnd.Next(1, allEntrances.Count)];
			}
		}

		public void WriteDebugInformation(string source, string message)
		{
			var part1 = FormatDebugInformation(string.Format("Source: [{0}]", source));
			var part2 = FormatDebugInformation(string.Format("  ->: {0}", message));
			string realMsg = string.Format("{0}{1}", part1, part2);
			FireDebugInfoEvent(realMsg);
		}
		public void WriteDebugInformation(string source, string message, params object[] args)
		{
			string substitutedMsg = string.Format(message, args);
			WriteDebugInformation(source, substitutedMsg);
		}

		public void Dispose()
		{
			DestroyWorld();
		}
	}
}
