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
			var engine = new InhabitantPositionEngine(new DummyWorld());

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

	public class DummyWorld : IEcoWorld
	{
		public void AddPlayer(LivingEntityWithQualities player)
		{
			throw new NotImplementedException();
		}

		public event EventHandler<DebugInfoEventArgs> DebugInformation;

		public void DestroyWorld()
		{
			throw new NotImplementedException();
		}

		public event EventHandler<EntityExitEventArgs> EntityExited;

		public event EventHandler<InhabitantActionEventArgs> InhabitantPerformedAction;

		public IEnumerable<LivingEntityWithQualities> Inhabitants
		{
			get { throw new NotImplementedException(); }
		}

		public void StartWorld()
		{
			throw new NotImplementedException();
		}

		public Engine.MapEngine.Map WorldMap
		{
			get { throw new NotImplementedException(); }
		}

		public void WriteDebugInformation(string source, string message)
		{
			throw new NotImplementedException();
		}

		public void WriteDebugInformation(string source, string message, params object[] args)
		{
			throw new NotImplementedException();
		}
	}

}
