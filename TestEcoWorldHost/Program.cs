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
using System.IO;
using System.Timers;

namespace TestEcoWorldHost
{
	class Program
	{
		static EcoWorld _world;
		static Timer _playerTimer = new Timer();

		static void Main(string[] args)
		{
			_playerTimer.Interval = 6000;  // add a 2nd player after 6 seconds
			_playerTimer.Elapsed += new ElapsedEventHandler(playerTimer_Elapsed);
			_playerTimer.Start();

			if (File.Exists("WorldDebugInfo.log"))
			{
				File.Delete("WorldDebugInfo.log");
			}
			//_world = CreateWorld();
			_world = CreateWorld2();

			_world.DebugInformation += new EventHandler<DebugInfoEventArgs>(world_DebugInformation);
			_world.EntityExited += new EventHandler<EntityExitEventArgs>(world_EntityExited);
			var player = CreatePlayer();

			WriteDebuggingInfo(_world);

			_world.AddPlayer(player);
			_world.StartWorld();

			Console.WriteLine("World Started. Hit ENTER to stop");
			Console.ReadLine();

			_world.DestroyWorld();
		}

		static void playerTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_playerTimer.Stop();
			var player2 = new LivingEntityWithQualities();
			player2.Qualities.Intelligence = 50;
			player2.Qualities.Sight = 50;
			player2.Qualities.Strength = 50;
			player2.Qualities.Speed = 50;
			player2.Entity = new Player2();

			_world.AddPlayer(player2);
		}

		static void world_EntityExited(object sender, EntityExitEventArgs e)
		{
			Console.WriteLine("\nCongrats {0}! You have completed this world!\n", e.Inhabitant.Entity.Name);
		}

		static void world_DebugInformation(object sender, DebugInfoEventArgs e)
		{
			Console.Write(e.DebugInformation);
			using (var file = File.Open("WorldDebugInfo.log", FileMode.Append))
			{
				var data = ASCIIEncoding.ASCII.GetBytes(e.DebugInformation);
				file.Write(data, 0, data.Length);
			}
		}

		private static void WriteDebuggingInfo(EcoWorld world)
		{
			System.IO.File.WriteAllText(".\\WorldMapDebug.txt", world.WorldMap.ToString());
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

			var world = new EcoWorld("TestWorld", map, null,true);
			return world;
		}
		private static EcoWorld CreateWorld2()
		{
			Map map = new Map(10, 10, 1);

			// setup entry and exit points
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(9, 9, 0, new MapExitBlock());

			// Setup some barrier blocks
			map.Set(3, 4, 0, new SolidBlock());
			map.Set(6, 7, 0, new SolidBlock());
			map.Set(3, 0, 0, new SolidBlock());
			map.Set(3, 1, 0, new SolidBlock());
			map.Set(3, 2, 0, new SolidBlock());
			map.Set(2, 2, 0, new SolidBlock());

			map.InitialiseMap();

			var world = new EcoWorld("TestWorld", map, null,true);
			return world;
		}
	}
}
