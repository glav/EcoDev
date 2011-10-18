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

namespace EcoDev.Engine.WorldEngine
{
	public class EcoWorld : IWorld
	{
		Map _worldMap;
		CancellationToken _worldCancelToken = new CancellationToken();
		List<LivingEntityWithQualities> _inhabitants = new List<LivingEntityWithQualities>();
		string _worldName;

		public EcoWorld(string worldName, Map worldMap, LivingEntityWithQualities[] inhabitants)
		{
			_worldName = worldName;

			if (worldMap == null)
			{
				throw new ArgumentException("World Map cannot be NULL");
			}
			_worldMap = worldMap;

			if (inhabitants != null && inhabitants.Length > 0)
			{
				_inhabitants.AddRange(inhabitants);
			}

		}

		public Map WorldMap { get { return _worldMap; } }

		public void AddPlayer(LivingEntityWithQualities player)
		{
			player.Entity.World = this;
			_inhabitants.Add(player);
		}

		public void StartWorld()
		{
			Task.Factory.StartNew(new Action(WorldProcessingTask), _worldCancelToken);
		}

		internal void WorldProcessingTask()
		{
			Trace.WriteLine("Starting World Processing: [{0}]", _worldName);

			while (!_worldCancelToken.IsCancellationRequested)
			{
				//TODO: Go through all players. If world == null, then place players at start and allow a movement

				//TODO: then cycle through players/inhabitants and process movements
			}

		}
	}
}
