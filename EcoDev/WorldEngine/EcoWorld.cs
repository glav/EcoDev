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

namespace EcoDev.Engine.WorldEngine
{
	public class EcoWorld : IWorld
	{
		Map _worldMap;
		CancellationTokenSource _tokenSource;
		List<LivingEntityWithQualities> _inhabitants = new List<LivingEntityWithQualities>();
		string _worldName;
		Task _worldTask = null;

		public EcoWorld(string worldName, Map worldMap, LivingEntityWithQualities[] inhabitants)
		{
			_worldName = worldName;

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
		public IEnumerable<LivingEntityWithQualities> Inhabitants{get { return _inhabitants; }}

		public void AddPlayer(LivingEntityWithQualities player)
		{
			player.Entity.World = this;
			_inhabitants.Add(player);
		}

		public void StartWorld()
		{
			_worldTask = Task.Factory.StartNew(new Action(WorldProcessingTask), _tokenSource.Token);
		}

		public void DestroyWorld()
		{
			Stopwatch cancelTimer = new Stopwatch();
			_tokenSource.Cancel();
			while (!_worldTask.IsCompleted && cancelTimer.ElapsedMilliseconds < 2000)
			{
			}

			if (!_worldTask.IsCompleted)
			{
				_worldTask.Dispose();
			}
		}

		internal void WorldProcessingTask()
		{
			Trace.WriteLine(string.Format("Starting World Processing: [{0}]", _worldName));

			while (!_tokenSource.IsCancellationRequested)
			{
				AddInhabitantsToMapIfRequired();
				//TODO: Go through all players. If world == null, then place players at start and allow a movement

				//TODO: then cycle through players/inhabitants and process movements
			}

		}

		internal void AddInhabitantsToMapIfRequired()
		{
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
						PerformEntityAction(entity);
					}
				}
			}
		}


		private void PerformEntityAction(LivingEntityWithQualities entity)
		{
			var positionContext = ConstructPositionContextForEntity(entity);
			ActionContext context = new ActionContext(positionContext);
			var actionResult = entity.Entity.DecideActionToPerform(context);
			ActOnEntityActionResult(entity,actionResult);
		}

		private void ActOnEntityActionResult(LivingEntityWithQualities entity, ActionResult actionResult)
		{
			var responseActionHandler = ActionResponseFactory.CreateActionResponseHandler(actionResult, entity);
			try
			{
				responseActionHandler.ExecuteActionToPerform();
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

		internal PositionContext ConstructPositionContextForEntity(LivingEntityWithQualities entity)
		{
			//TODO: NEed to take into account sight attribute when populating surrounding positions/mapblocks of an entity
			MapBlock currentPosition = _worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition);
			var fwdFacingBlocks = new List<MapBlock>();
			var rearFacingBlocks = new List<MapBlock>();
			var leftFacingBlocks = new List<MapBlock>();
			var rightFacingBlocks = new List<MapBlock>();

			switch (entity.ForwardFacingAxis)
			{
				case WorldAxis.PositiveX:
					fwdFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition + 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition - 1,entity.PositionInMap.yPosition,entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition,entity.PositionInMap.yPosition+1,entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition,entity.PositionInMap.yPosition-1,entity.PositionInMap.zPosition));
					break;
				case WorldAxis.PositiveY:
					fwdFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + 1, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition,entity.PositionInMap.yPosition-1,entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition-1,entity.PositionInMap.yPosition+1,entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition + 1,entity.PositionInMap.yPosition,entity.PositionInMap.zPosition));
					break;
				case WorldAxis.PositiveZ:
					break;
				case WorldAxis.NegativeX:
					fwdFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition - 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition + 1,entity.PositionInMap.yPosition,entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition,entity.PositionInMap.yPosition-1,entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition,entity.PositionInMap.yPosition+1,entity.PositionInMap.zPosition));
					break;
				case WorldAxis.NegativeY:
					fwdFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - 1, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition,entity.PositionInMap.yPosition+1,entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition+1,entity.PositionInMap.yPosition+1,entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(_worldMap.Get(entity.PositionInMap.xPosition - 1,entity.PositionInMap.yPosition,entity.PositionInMap.zPosition));
					break;
				case WorldAxis.NegativeZ:
					break;
			}

			var posContext = new PositionContext(currentPosition,fwdFacingBlocks.ToArray(),rearFacingBlocks.ToArray(),leftFacingBlocks.ToArray(),rightFacingBlocks.ToArray());
			return posContext;
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
	}
}
