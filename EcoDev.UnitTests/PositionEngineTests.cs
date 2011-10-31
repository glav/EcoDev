using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoDev.Engine.WorldEngine;
using EcoDev.Engine.Entities;
using EcoDev.Core.Common.Maps;
using EcoDev.Core.Common;

namespace EcoDev.UnitTests
{
	[TestClass]
	public class PositionEngineTests
	{
		[TestMethod]
		public void NextRelativePositionShouldBeValid()
		{
			var engine = new InhabitantPositionEngine();
			
			var entity = new LivingEntityWithQualities();
			entity.PositionInMap = new MapPosition(5,5,0);

			entity.ForwardFacingAxis = WorldAxis.PositiveX;
			var nextPosition = engine.GetNextRelativePositionInMap(entity, MovementDirection.Forward);
			Assert.AreEqual<int>(6, nextPosition.xPosition);

			entity.ForwardFacingAxis = WorldAxis.NegativeX;
			nextPosition = engine.GetNextRelativePositionInMap(entity, MovementDirection.Forward);
			Assert.AreEqual<int>(4, nextPosition.xPosition);

			entity.ForwardFacingAxis = WorldAxis.PositiveY;
			nextPosition = engine.GetNextRelativePositionInMap(entity, MovementDirection.Forward);
			Assert.AreEqual<int>(6, nextPosition.yPosition);

			entity.ForwardFacingAxis = WorldAxis.NegativeY;
			nextPosition = engine.GetNextRelativePositionInMap(entity, MovementDirection.Forward);
			Assert.AreEqual<int>(4, nextPosition.yPosition);

			entity.ForwardFacingAxis = WorldAxis.PositiveY;
			nextPosition = engine.GetNextRelativePositionInMap(entity, MovementDirection.Left);
			Assert.AreEqual<int>(4, nextPosition.xPosition);

			entity.ForwardFacingAxis = WorldAxis.NegativeX;
			nextPosition = engine.GetNextRelativePositionInMap(entity, MovementDirection.Right);
			Assert.AreEqual<int>(6, nextPosition.yPosition);

		}
	}
}
