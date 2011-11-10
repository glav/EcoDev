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
using EcoDev.Visualisation.Basic;

namespace TestEcoWorldHost
{
	class Program
	{
		static EcoDev.Engine.WorldEngine.EcoWorld _world;
		static Timer _playerTimer = new Timer();
		static VisualisationEngine _visualiser;

		static void Main(string[] args)
		{

			if (File.Exists("WorldDebugInfo.log"))
			{
				File.Delete("WorldDebugInfo.log");
			}
			//_world = StartReallySimpleWorld();
			//_world = StartModeratelyComplexWorld();

			//_world = StartMuchMoreComplexWorld();
			_world = StartVeryComplexMultiPlayerWorld();

			_visualiser = new VisualisationEngine(_world);

			_world.DebugInformation += new EventHandler<DebugInfoEventArgs>(world_DebugInformation);
			_world.EntityExited += new EventHandler<EntityExitEventArgs>(world_EntityExited);
			_world.InhabitantPerformedAction += new EventHandler<InhabitantActionEventArgs>(_world_InhabitantPerformedAction);
			
			// Our initial player
			var player = CreatePlayer();

			WriteDebuggingInfo(_world);

			_world.AddPlayer(player);
			_world.StartWorld();

			Console.WriteLine("World Started. Hit ENTER to stop");
			_visualiser.StartMapRendering();
			Console.ReadLine();

			_visualiser.StopMapRendering();
			_world.DestroyWorld();
		}


		static void _world_InhabitantPerformedAction(object sender, InhabitantActionEventArgs e)
		{
			//throw new NotImplementedException();
		}

		static void playerTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_playerTimer.Stop();
			var player2 = new LivingEntityWithQualities();
			player2.Qualities.Intelligence = 50;
			player2.Qualities.Sight = 150;
			player2.Qualities.Strength = 50;
			player2.Qualities.Speed = 180;
			player2.Entity = new Player2();

			_world.AddPlayer(player2);
		}

		static void playerTimer_Elapsed_ForReallyComplexWorld(object sender, ElapsedEventArgs e)
		{
			_playerTimer.Stop();
			var player2 = new LivingEntityWithQualities();
			player2.Qualities.Intelligence = 50;
			player2.Qualities.Sight = 150;
			player2.Qualities.Strength = 50;
			player2.Qualities.Speed = 90;
			player2.Entity = new Player2();
			var player3 = new LivingEntityWithQualities();
			player3.Qualities.Intelligence = 50;
			player3.Qualities.Sight = 150;
			player3.Qualities.Strength = 50;
			player3.Qualities.Speed = 180;
			player3.Entity = new Player3();

			_world.AddPlayer(player2);
			_world.AddPlayer(player3);
		}

		static void world_EntityExited(object sender, EntityExitEventArgs e)
		{
			Console.WriteLine("\nCongrats {0}! You have completed this world!\n", e.Inhabitant.Entity.Name);
		}

		static void world_DebugInformation(object sender, DebugInfoEventArgs e)
		{
			//Console.Write(e.DebugInformation);
			using (var file = File.Open("WorldDebugInfo.log", FileMode.Append))
			{
			    var data = ASCIIEncoding.ASCII.GetBytes(e.DebugInformation);
			    file.Write(data, 0, data.Length);
			}
		}

		private static void WriteDebuggingInfo(EcoDev.Engine.WorldEngine.EcoWorld world)
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

		private static EcoDev.Engine.WorldEngine.EcoWorld StartReallySimpleWorld()
		{
			Map map = new Map(10, 10, 1);
			
			// setup entry and exit points
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(9, 9, 0, new MapExitBlock());

			// Setup some barrier blocks
			map.Set(3, 4, 0, new SolidBlock());
			map.Set(6, 7, 0, new SolidBlock());

			map.InitialiseMap();

			var world = new EcoDev.Engine.WorldEngine.EcoWorld("TestWorld", map, null, true);
			return world;
		}
		private static EcoDev.Engine.WorldEngine.EcoWorld StartModeratelyComplexWorld()
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

			var world = new EcoDev.Engine.WorldEngine.EcoWorld("TestWorld", map, null, true);
			return world;
		}
		private static EcoDev.Engine.WorldEngine.EcoWorld StartMuchMoreComplexWorld()
		{
			Map map = new Map(20, 20, 1);

			// setup entry and exit points
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(19, 19, 0, new MapExitBlock());

			// Setup some barrier blocks
			map.Set(3, 4, 0, new SolidBlock());
			map.Set(6, 7, 0, new SolidBlock());
			map.Set(3, 0, 0, new SolidBlock());
			map.Set(3, 1, 0, new SolidBlock());
			map.Set(3, 2, 0, new SolidBlock());
			map.Set(2, 2, 0, new SolidBlock());
			map.Set(5, 9, 0, new SolidBlock());
			map.Set(5, 19, 0, new SolidBlock());
			map.Set(14, 19, 0, new SolidBlock());
			map.Set(14, 18, 0, new SolidBlock());

			map.InitialiseMap();

			_playerTimer.Interval = 6000;  // add a 2nd player after 6 seconds
			_playerTimer.Elapsed += new ElapsedEventHandler(playerTimer_Elapsed);
			_playerTimer.Start();


			var world = new EcoDev.Engine.WorldEngine.EcoWorld("TestWorld", map, null, true);
			return world;
		}

		private static EcoDev.Engine.WorldEngine.EcoWorld StartVeryComplexMultiPlayerWorld()
		{
			Map map = new Map(30, 30, 1);

			// setup entry and exit points
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(25, 29, 0, new MapExitBlock());

			// Setup some barrier blocks
			map.Set(3, 4, 0, new SolidBlock());
			map.Set(6, 7, 0, new SolidBlock());
			map.Set(3, 0, 0, new SolidBlock());
			map.Set(3, 1, 0, new SolidBlock());
			map.Set(3, 2, 0, new SolidBlock());
			map.Set(2, 2, 0, new SolidBlock());
			map.Set(5, 9, 0, new SolidBlock());
			map.Set(5, 19, 0, new SolidBlock());
			map.Set(14, 19, 0, new SolidBlock());
			map.Set(14, 29, 0, new SolidBlock());
			map.Set(14, 28, 0, new SolidBlock());
			map.Set(14, 27, 0, new SolidBlock());
			map.Set(14, 26, 0, new SolidBlock());
			map.Set(14, 25, 0, new SolidBlock());
			map.Set(14, 24, 0, new SolidBlock());
			map.Set(29, 20, 0, new SolidBlock());
			map.Set(28, 20, 0, new SolidBlock());
			map.Set(27, 20, 0, new SolidBlock());

			map.InitialiseMap();

			_playerTimer.Interval = 7000;  // add a 2nd and 3rd player after 7 seconds
			_playerTimer.Elapsed += new ElapsedEventHandler(playerTimer_Elapsed_ForReallyComplexWorld);
			_playerTimer.Start();


			var world = new EcoDev.Engine.WorldEngine.EcoWorld("TestWorld", map, null, true);
			return world;
		}

	}
}
