using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.Entities;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common.Maps;
using EcoDev.Core.Common.Actions;
using EcoDev.Core.Common;

namespace EcoDev.Engine.WorldEngine
{
	public class InhabitantPositionEngine
	{
		internal PositionContext ConstructPositionContextForEntity(LivingEntityWithQualities entity, Map worldMap)
		{
			//TODO: NEed to take into account sight attribute when populating surrounding positions/mapblocks of an entity
			MapBlock currentPosition = worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition);
			var fwdFacingBlocks = new List<MapBlock>();
			var rearFacingBlocks = new List<MapBlock>();
			var leftFacingBlocks = new List<MapBlock>();
			var rightFacingBlocks = new List<MapBlock>();

			switch (entity.ForwardFacingAxis)
			{
				case WorldAxis.PositiveX:
					fwdFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition + 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition - 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + 1, entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - 1, entity.PositionInMap.zPosition));
					break;
				case WorldAxis.PositiveY:
					fwdFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + 1, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - 1, entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition - 1, entity.PositionInMap.yPosition + 1, entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition + 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					break;
				case WorldAxis.PositiveZ:
					break;
				case WorldAxis.NegativeX:
					fwdFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition - 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition + 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - 1, entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + 1, entity.PositionInMap.zPosition));
					break;
				case WorldAxis.NegativeY:
					fwdFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - 1, entity.PositionInMap.zPosition));
					rearFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + 1, entity.PositionInMap.zPosition));
					leftFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition + 1, entity.PositionInMap.yPosition + 1, entity.PositionInMap.zPosition));
					rightFacingBlocks.Add(worldMap.Get(entity.PositionInMap.xPosition - 1, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					break;
				case WorldAxis.NegativeZ:
					break;
			}

			var posContext = new PositionContext(currentPosition, fwdFacingBlocks.ToArray(), rearFacingBlocks.ToArray(), leftFacingBlocks.ToArray(), rightFacingBlocks.ToArray());
			return posContext;
		}

		internal MapPosition GetNextRelativePositionInMap(LivingEntityWithQualities entity, MovementDirection direction)
		{
			var nextPosition = new MapPosition(entity.PositionInMap);
			switch (entity.ForwardFacingAxis)
			{
				case WorldAxis.PositiveX:
				case WorldAxis.NegativeX:
					if (direction == MovementDirection.Forward)
					{
						nextPosition.xPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveX) ? 1 : -1);
					}
					if (direction == MovementDirection.Back)
					{
						nextPosition.xPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveX) ? -1 : 1);
					}
					if (direction == MovementDirection.Left)
					{
						nextPosition.yPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveX) ? 1 : -1);
					}
					if (direction == MovementDirection.Right)
					{
						nextPosition.yPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveX) ? -1 : 1);
					}
					break;
				case WorldAxis.NegativeY:
				case WorldAxis.PositiveY:
					if (direction == MovementDirection.Forward)
					{
						nextPosition.yPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveY) ? 1 : -1);
					}
					if (direction == MovementDirection.Back)
					{
						nextPosition.xPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveY) ? -1 : 1);
					}
					if (direction == MovementDirection.Left)
					{
						nextPosition.xPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveY) ? -1 : 1);
					}
					if (direction == MovementDirection.Right)
					{
						nextPosition.xPosition += ((entity.ForwardFacingAxis == WorldAxis.PositiveY) ? 1 : -1);
					}
					break;
				case WorldAxis.NegativeZ:
				case WorldAxis.PositiveZ:
					throw new NotImplementedException("Z axis not catered for yet");
					break;
			}
			return nextPosition;

		}
	}

}
