﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoDev.Core.Common;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common.BuildingBlocks;
using System.Diagnostics;
using EcoDev.Engine.WorldEngine;
using EcoDev.Core.Common.Actions;
using EcoDev.Engine.Entities;

namespace EcoDev.UnitTests
{
	[TestClass]
	public class WorldTests
	{
		[TestMethod]
		public void WorldCanFindAnEntrance()
		{
			var world = GetTestWorld();

			var entrance = world.FindAnEntrance();

			Assert.IsNotNull(entrance);
		}

		[TestMethod]
		public void WorldCanDetermineForwardFacingAxis()
		{
			var map = new Map(10, 10, 1);
			map.Set(9, 0, 0, new MapEntranceBlock());
			map.Set(9, 4, 0, new MapExitBlock());

			var player = GetPlayer();

			var world = new EcoWorld("test", map, new LivingEntityWithQualities[] { player });
			var entrance = world.FindAnEntrance();
			player.ForwardFacingAxis = WorldAxis.NegativeZ; // This is definitely wrong
			world.DetermineForwardFacingAxis(entrance, player);

			Assert.AreEqual<WorldAxis>(WorldAxis.NegativeX, player.ForwardFacingAxis);

			map = new Map(10, 10, 1);
			map.Set(0, 9, 0, new MapEntranceBlock());
			map.Set(9, 4, 0, new MapExitBlock());
			world = new EcoWorld("test", map, new LivingEntityWithQualities[] { player });
			entrance = world.FindAnEntrance();
			player.ForwardFacingAxis = WorldAxis.NegativeZ; // This is definitely wrong
			world.DetermineForwardFacingAxis(entrance, player);
			Assert.AreEqual<WorldAxis>(WorldAxis.PositiveX, player.ForwardFacingAxis);

			map = new Map(10, 10, 1);
			map.Set(0, 0, 0, new MapEntranceBlock());
			map.Set(9, 4, 0, new MapExitBlock());
			world = new EcoWorld("test", map, new LivingEntityWithQualities[] { player });
			entrance = world.FindAnEntrance();
			player.ForwardFacingAxis = WorldAxis.NegativeZ; // This is definitely wrong
			world.DetermineForwardFacingAxis(entrance, player);
			Assert.AreEqual<WorldAxis>(WorldAxis.PositiveX, player.ForwardFacingAxis);

			map = new Map(10, 10, 1);
			map.Set(4, 9, 0, new MapEntranceBlock());
			map.Set(9, 4, 0, new MapExitBlock());
			world = new EcoWorld("test", map, new LivingEntityWithQualities[] { player });
			entrance = world.FindAnEntrance();
			player.ForwardFacingAxis = WorldAxis.NegativeZ; // This is definitely wrong
			world.DetermineForwardFacingAxis(entrance, player);
			Assert.AreEqual<WorldAxis>(WorldAxis.NegativeY, player.ForwardFacingAxis);
		}

		[TestMethod]
		public void WorldCanConstructValidPositionContext()
		{
			var world = GetTestWorld();

			Assert.IsNotNull(world);

			world.AddInhabitantsToMapIfRequired();

			var playerInWorld = world.Inhabitants.First();
			var posContext = world.ConstructPositionContextForEntity(playerInWorld);

			Assert.IsNotNull(posContext);

			// We should always be able to see something
			Assert.IsTrue(posContext.RearFacingPositions.Count() > 0);
			Assert.IsTrue(posContext.RightFacingPositions.Count() > 0);
			Assert.IsTrue(posContext.ForwardFacingPositions.Count() > 0);
			Assert.IsTrue(posContext.LeftFacingPositions.Count() > 0);

			// Since we are positioned at an entrance, the current position should be that entrance
			// and the rear position should be a solid block (ie. cannot move to)
			Assert.IsTrue(posContext.CurrentPosition is MapEntranceBlock);
			Assert.IsTrue(posContext.ForwardFacingPositions[0] == null); // fwd is an empty space
			Assert.IsTrue(posContext.RearFacingPositions[0] is SolidBlock);  // rear is a solid block

			// Based on orientation, left is empty, right is solid (edge of map)
			Assert.IsTrue(posContext.LeftFacingPositions[0] is SolidBlock ); // fwd is an empty space
			Assert.IsTrue(posContext.RightFacingPositions[0] == null);  // rear is a solid block

		}


		private EcoWorld GetTestWorld()
		{
			var map = new Map(10, 10, 1);
			map.Set(9, 0, 0, new MapEntranceBlock());
			map.Set(9, 4, 0, new MapExitBlock());

			var player = GetPlayer();

			var world = new EcoWorld("test", map, new LivingEntityWithQualities[] { player });

			return world;
		}

		private static LivingEntityWithQualities GetPlayer()
		{
			var player = new LivingEntityWithQualities();
			player.Entity = new DummyPlayer();
			return player;
		}
	}


	public class DummyPlayer : LivingEntity
	{
		public DummyPlayer()
		{
			LifeKey = new Guid("60909948-E9AF-4bb5-B59C-084E9BC41348");
		}
		public override ActionResult DecideActionToPerform(ActionContext actionContext)
		{
			var actionResult = new MovementAction();
			return actionResult;
		}
	}

}
