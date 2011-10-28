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

namespace TestEcoWorldHost
{
	class Program
	{
		static void Main(string[] args)
		{
			var world = CreateWorld();
			world.DebugInformation += new EventHandler<DebugInfoEventArgs>(world_DebugInformation);
			world.EntityExited += new EventHandler<EntityExitEventArgs>(world_EntityExited);
			var player = CreatePlayer();

			WriteDebuggingInfo(world);

			world.AddPlayer(player);
			world.StartWorld();

			Console.WriteLine("World Started. Hit ENTER to stop");
			Console.ReadLine();

			world.DestroyWorld();
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

			var world = new EcoWorld("TestWorld", map, null);
			return world;
		}
	}
}
