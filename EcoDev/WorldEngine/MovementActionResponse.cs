using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common;
using EcoDev.Core.Common.Maps;
using EcoDev.Core.Common.BuildingBlocks;
using EcoDev.Core.Common.Actions;

namespace EcoDev.Engine.WorldEngine
{
	internal class MovementActionResponse : ActionResponse
	{
		InhabitantPositionEngine _positionEngine = new InhabitantPositionEngine();
		protected override void HandleActionToPerform()
		{
			var positionCtxt = _positionEngine.ConstructPositionContextForEntity(Inhabitant,World.WorldMap);
			MapBlock blockToMoveTo = GetPotentialBlockToMoveToInMap(positionCtxt);

			bool canMove = false;
			if (blockToMoveTo == null)
			{
				canMove = true;
			}
			else
			{
				if (blockToMoveTo is MapExitBlock)
				{
					canMove = true;
				} else if (blockToMoveTo.IsUnmoveable || blockToMoveTo is SolidBlock)
				{
					canMove = false;
				}
			}

			if (canMove)
			{
				if (NoPlayersCurrentlyOccupyPosition())
				{
					MovePlayerToBlockInRequestedDirection();
				}
			}
		}

		private bool NoPlayersCurrentlyOccupyPosition()
		{
			var nextPosition = _positionEngine.GetNextRelativePositionInMap(Inhabitant, DecidedAction.DirectionToMove);
			var players = World.Inhabitants.ToList();
			for (int cnt = 0; cnt < players.Count; cnt++)
			{
				var testInhabitant = players[cnt];
				if (testInhabitant.PositionInMap == nextPosition && testInhabitant.Entity.LifeKey != Inhabitant.Entity.LifeKey)
				{
					World.WriteDebugInformation(Inhabitant.Entity.Name, "Cannot move to position: {0} as occupied by entity: [{1}]",nextPosition, testInhabitant.Entity.Name);
					return false;
				}
			}
			return true;
		}

		private void MovePlayerToBlockInRequestedDirection()
		{
			World.WriteDebugInformation(Inhabitant.Entity.Name,"Moving in Direction: {0}", DecidedAction.DirectionToMove);

			var nextPosition = _positionEngine.GetNextRelativePositionInMap(Inhabitant, DecidedAction.DirectionToMove);
			Inhabitant.PositionInMap = nextPosition;

			World.WriteDebugInformation(Inhabitant.Entity.Name, "New Position in map: {0}", Inhabitant.PositionInMap);
		}

		private MapBlock GetPotentialBlockToMoveToInMap(PositionContext positionCtxt)
		{
			MapBlock blockToMoveTo = null;
			switch (DecidedAction.DirectionToMove)
			{
				case MovementDirection.Forward:
					blockToMoveTo = positionCtxt.ForwardFacingPositions[0];
					break;
				case MovementDirection.Back:
					blockToMoveTo = positionCtxt.RearFacingPositions[0];
					break;
				case MovementDirection.Left:
					blockToMoveTo = positionCtxt.LeftFacingPositions[0];
					break;
				case MovementDirection.Right:
					blockToMoveTo = positionCtxt.RightFacingPositions[0];
					break;
				case MovementDirection.Up:
					throw new NotImplementedException("Up movement not handled yet");
					break;
				case MovementDirection.Down:
					throw new NotImplementedException("Down movement not handled yet");
					break;
			}
			return blockToMoveTo;
		}
	}
}
