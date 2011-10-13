using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoDev.Core.Common;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common.BuildingBlocks;

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

			map.CreateWorld();
		}
		

		[TestMethod]
		public void MapWithNoEntryShouldNotValidate()
		{
			var map = new Map(10, 10, 10);
			map.Set(9, 9, 9, new MapExitBlock());

			try
			{
				map.CreateWorld();
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
				map.CreateWorld();
				Assert.Fail("Map should not validate without an exit point.");
			}
			catch (MapInvalidException mex) { }
		}
		
		[TestMethod]
		public void MapShouldOutputSimpleDebugRepresentation()
		{
			var map = new Map(10, 10, 1);
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(9, 9, 0, new MapExitBlock());

			map.CreateWorld();
			var mapOutput = map.ToString();

			Assert.IsNotNull(mapOutput);
			Assert.IsTrue(mapOutput.Contains(new MapEntranceBlock().ToString()));
			Assert.IsTrue(mapOutput.Contains(new MapExitBlock().ToString()));
		}
	}
}
