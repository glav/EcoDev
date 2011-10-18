using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.Entities;
using EcoDev.Engine.WorldEngine;
using EcoDev.Core.Common;
using EcoDev.Core.Common.Actions;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common.BuildingBlocks;

namespace TestEcoWorldHost
{
	class Program
	{
		static void Main(string[] args)
		{
			var world = CreateWorld();
			var player = CreatePlayer();

			WriteDebuggingInfo(world);

			world.AddPlayer(player);
			world.StartWorld();
		}

		private static void WriteDebuggingInfo(EcoWorld world)
		{
			System.IO.File.WriteAllText(".\\WorldDebug.txt", world.WorldMap.ToString());
		}

		private static LivingEntityWithQualities CreatePlayer()
		{
			var player = new LivingEntityWithQualities();
			player.Qualities.Intelligence = 50;
			player.Qualities.Sight = 50;
			player.Qualities.Strength = 50;
			player.Qualities.Speed = 50;
			player.Entity = new TestPlayer();

			return player;
		}

		private static EcoWorld CreateWorld()
		{
			Map map = new Map(10, 10, 1);
			
			// setup entry and exit points
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(9, 9, 0, new MapExitBlock());

			// Setup some barrier blocks
			map.Set(3, 4, 0, new SolidBlock());
			map.Set(6, 7, 0, new SolidBlock());

			map.InitialiseMap();

			var world = new EcoWorld("TestWorld", map, null);
			return world;
		}
	}
}
