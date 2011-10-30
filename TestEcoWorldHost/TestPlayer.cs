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

				// Keep going in the same direction if possible
				if (PreviousAction != null)
				{
					MapBlock nextBlockBAsedOnPreviousMovement = null;
					switch (PreviousAction.DirectionToMove)
					{
						case MovementDirection.Forward:
							nextBlockBAsedOnPreviousMovement = actionContext.Position.ForwardFacingPositions[0];
							break;
						case MovementDirection.Back:
							nextBlockBAsedOnPreviousMovement = actionContext.Position.RearFacingPositions[0];
							break;
						case MovementDirection.Left:
							nextBlockBAsedOnPreviousMovement = actionContext.Position.LeftFacingPositions[0];
							break;
						case MovementDirection.Right:
							nextBlockBAsedOnPreviousMovement = actionContext.Position.RightFacingPositions[0];
							break;
					}
					World.WriteDebugInformation(Name, string.Format("Attempting to move {0} based on previous action to block {1}", PreviousAction.DirectionToMove, nextBlockBAsedOnPreviousMovement != null ? nextBlockBAsedOnPreviousMovement.GetType().ToString() : "Empty"));
					
					
					// If we were moving back, then try going left or right first
					if (PreviousAction.DirectionToMove == MovementDirection.Back)
					{
						if (CheckAccessibilityOfMapBlock(actionContext.Position.LeftFacingPositions[0]))
						{
							nextBlockBAsedOnPreviousMovement = actionContext.Position.LeftFacingPositions[0];
							action.DirectionToMove = MovementDirection.Left;
							return action;
						}
					}
					// If we can keep moving in the same direction, then do it.
					// Elselet it flow through to normal directional logic
					if (CheckAccessibilityOfMapBlock(nextBlockBAsedOnPreviousMovement))
					{
						action.DirectionToMove = PreviousAction.DirectionToMove;

						World.WriteDebugInformation(Name,string.Format("Moving {0} based on previous action", action.DirectionToMove));
						return action;
					}
				}

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
				if (actionContext.Position.RearFacingPositions.Length > 0)
				{
					if (CheckAccessibilityOfMapBlock(actionContext.Position.RearFacingPositions[0]))
					{
						action.DirectionToMove = MovementDirection.Back;
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

	public class Player2 : TestPlayer
	{
		public Player2()
		{
			Name = "Tommy test";
			LifeKey = Guid.NewGuid();
			Size = new EntitySize() { Height = 1, Thickness = 1, Width = 1 };
			Weight = 10;

		}
	}
}
