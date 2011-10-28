using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common;
using EcoDev.Core.Common.Actions;
using EcoDev.Core.Common.BuildingBlocks;
using EcoDev.Core.Common.Maps;

namespace TestEcoWorldHost
{
	public class TestPlayer:LivingEntity
	{
		public TestPlayer()
		{
			Name = "Beta Boy";
			LifeKey = Guid.NewGuid();
			Size = new EntitySize() { Height = 1, Thickness=1, Width = 1};
			Weight = 10;

		}
		public override ActionResult DecideActionToPerform(EcoDev.Core.Common.Actions.ActionContext actionContext)
		{
			try
			{
				var action = new MovementAction();

				// move forward if we can
				if (actionContext.Position.ForwardFacingPositions.Length > 0)
				{
					if (CheckAccessibilityOfMapBlock(actionContext.Position.ForwardFacingPositions[0]))
					{
						action.DirectionToMove = MovementDirection.Forward;
						return action;
					}
				}
				if (actionContext.Position.LeftFacingPositions.Length > 0)
				{
					if (CheckAccessibilityOfMapBlock(actionContext.Position.LeftFacingPositions[0]))
					{
						action.DirectionToMove = MovementDirection.Left;
						return action;
					}
				}
				if (actionContext.Position.RightFacingPositions.Length > 0)
				{
					if (CheckAccessibilityOfMapBlock(actionContext.Position.RightFacingPositions[0]))
					{
						action.DirectionToMove = MovementDirection.Right;
						return action;
					}
				}
				if (actionContext.Position.RearFacingPositions.Length > 0)
				{
					if (CheckAccessibilityOfMapBlock(actionContext.Position.RearFacingPositions[0]))
					{
						action.DirectionToMove = MovementDirection.Back;
						return action;
					}
				}

				return action;
			}
			catch (Exception ex)
			{
				World.WriteDebugInformation("Player: "+ Name, string.Format("Player Generated exception: {0}",ex.Message));
				throw ex;
			}
		}

		private bool CheckAccessibilityOfMapBlock(MapBlock block)
		{
			if (block == null || block.Accessibility == MapBlockAccessibility.AllowEntry || block.Accessibility == MapBlockAccessibility.AllowExit || block.Accessibility == MapBlockAccessibility.AllowPotentialEntry)
			{
				return true;
			}
			return false;
		}

	}
}
