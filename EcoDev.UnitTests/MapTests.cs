using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoDev.Core.Common;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common.BuildingBlocks;
using System.Diagnostics;

namespace EcoDev.UnitTests
{
	[TestClass]
	public class MapTests
	{
		[TestMethod]
		public void MapShouldNotAllowInvalidDimensions()
		{
			try
			{
				var map = new Map(0, 1, 1);
				Assert.Fail("Map should not allow zero or negative width");
			}
			catch (ArgumentOutOfRangeException aex) { }
			try
			{
				var map = new Map(1, 0, 1);
				Assert.Fail("Map should not allow zero or negative height");
			}
			catch (ArgumentOutOfRangeException aex) { }
			try
			{
				var map = new Map(1, 1, 0);
				Assert.Fail("Map should not allow zero or negative depth");
			}
			catch (ArgumentOutOfRangeException aex) { }

		}

		[TestMethod]
		public void MapShouldNotAllowInvalidPositions()
		{
			var map = new Map(10, 10, 1);
			map.Set(0, 0, 0, new SolidBlock());
			try
			{
				// This wont work because it is all zero based
				map.Set(1, 1, 1, new SolidBlock());
				Assert.Fail("Map should not allow invalid position");
			}
			catch (ArgumentOutOfRangeException aex) { }
			
			try
			{
				map.Set(11, 1, 1, new SolidBlock());
				Assert.Fail("Map should not allow invalid x position");
			}
			catch (ArgumentOutOfRangeException aex ) {}
			try
			{
				map.Set(1, 11, 1, new SolidBlock());
				Assert.Fail("Map should not allow invalid y position");
			}
			catch  (ArgumentOutOfRangeException aex ) {}
			try
			{
				map.Set(1, 1, -1, new SolidBlock());
				Assert.Fail("Map should not allow invalid z position");
			}
			catch  (ArgumentOutOfRangeException aex ) {}
			try
			{
				map.Set(1, 1, 2, new SolidBlock());
				Assert.Fail("Map should not allow invalid x position");
			}
			catch (ArgumentOutOfRangeException aex) { }


		}

		[TestMethod]
		public void MapShouldValidate()
		{
			var map = new Map(10, 10, 10);
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(9, 9, 9, new MapExitBlock());

			map.InitialiseMap();
		}
		

		[TestMethod]
		public void MapWithNoEntryShouldNotValidate()
		{
			var map = new Map(10, 10, 10);
			map.Set(9, 9, 9, new MapExitBlock());

			try
			{
				map.InitialiseMap();
				Assert.Fail("Map should not validate without an entry point.");
			}
			catch (MapInvalidException mex) { }
		}
		
		[TestMethod]
		public void MapWithNoExitShouldNotValidate()
		{
			var map = new Map(10, 10, 10);
			map.Set(0, 0, 0, new MapEntranceBlock());

			try
			{
				map.InitialiseMap();
				Assert.Fail("Map should not validate without an exit point.");
			}
			catch (MapInvalidException mex) { }
		}
		
		[TestMethod]
		public void MapShouldOutputSimpleDebugRepresentation()
		{
			var map = new Map(10, 10, 1);
			Assert.IsTrue(map.Set(0, 0, 0, new MapEntranceBlock()));
			Assert.IsTrue(map.Set(9, 9, 0, new MapExitBlock()));
			Assert.IsTrue(map.Set(2, 6, 0, new SolidBlock()));
			Assert.IsTrue(map.Set(1, 4, 0, new SolidBlock()));

			// Should be false as this position is already filled
			Assert.IsFalse(map.Set(1, 4, 0, new SolidBlock()));

			map.InitialiseMap();
			var mapOutput = map.ToString();

			Assert.IsNotNull(mapOutput);
			Assert.IsTrue(mapOutput.Contains(new MapEntranceBlock().ToString()));
			Assert.IsTrue(mapOutput.Contains(new MapExitBlock().ToString()));
		}

		[TestMethod]
		public void MapSetAndClearPositions()
		{
			var map = new Map(10, 10, 1);
			Assert.IsTrue(map.Set(0, 0, 0, new MapEntranceBlock()));
			Assert.IsTrue(map.Set(9, 9, 0, new MapExitBlock()));
			Assert.IsTrue(map.Set(2, 6, 0, new SolidBlock()));
			Assert.IsTrue(map.Set(1, 4, 0, new SolidBlock()));

			// Should be false as this position is already filled
			Assert.IsFalse(map.Set(1, 4, 0, new SolidBlock()));

			map.Clear(1, 4, 0);

			// Should be true now as this position was cleared
			Assert.IsTrue(map.Set(1, 4, 0, new SolidBlock()));

			map.ClearAll();

			try
			{
				// Now that everything has been cleared, this will fail
				// as there is no mandatory entrance and exit
				map.InitialiseMap();
				Assert.Fail("Creating a cleared map should fail as there is no entry or exit");
			}
			catch (MapInvalidException mex) { }
		}

		[TestMethod]
		public void LargeMapShouldPerformWell()
		{
			// The largest map
			try
			{
				var maxValueToUse = 300;
				Stopwatch watch = new Stopwatch();

				watch.Start();
				var map = new Map(maxValueToUse, maxValueToUse, maxValueToUse);

				map.Set(0, 0, 0, new MapEntranceBlock());
				map.Set(maxValueToUse - 2, maxValueToUse - 2, maxValueToUse-2, new MapExitBlock());
				map.InitialiseMap();

				watch.Stop();
				Console.WriteLine(string.Format("Map creation took {0} milliseconds", watch.ElapsedMilliseconds));
			}
			catch (OutOfMemoryException oex)
			{
				Assert.Inconclusive("You ran out of memory");
			}
		}
	}
}
