using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.Entities;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common.Maps;
using EcoDev.Core.Common.Actions;
using EcoDev.Core.Common;
using EcoDev.Core.Common.BuildingBlocks;

namespace EcoDev.Engine.WorldEngine
{
	public class InhabitantPositionEngine
	{
		private IEcoWorld _world;

		public InhabitantPositionEngine(IEcoWorld world)
		{
			if (world == null)
			{
				throw new ArgumentNullException("World cannot be NULL");
			}
			_world = world;
		}
		internal PositionContext ConstructPositionContextForEntity(LivingEntityWithQualities entity)
		{
			int relativeSight = entity.Qualities.RelativeSight;
			var worldMap = _world.WorldMap;

			MapBlock currentPosition = worldMap.Get(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition);
			var fwdFacingBlocks = new List<MapBlock>();
			var rearFacingBlocks = new List<MapBlock>();
			var leftFacingBlocks = new List<MapBlock>();
			var rightFacingBlocks = new List<MapBlock>();

			switch (entity.ForwardFacingAxis)
			{
				case WorldAxis.PositiveX:
					for (int blockCnt = 0; blockCnt <= relativeSight; blockCnt++)
					{
						int relativePos = 1 + blockCnt;
						fwdFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition + relativePos, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
						rearFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition - relativePos, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
						leftFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + relativePos, entity.PositionInMap.zPosition));
						rightFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - relativePos, entity.PositionInMap.zPosition));
					}
					break;
				case WorldAxis.PositiveY:
					for (int blockCnt = 0; blockCnt <= relativeSight; blockCnt++)
					{
						int relativePos = 1 + blockCnt;
						fwdFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + relativePos, entity.PositionInMap.zPosition));
						rearFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - relativePos, entity.PositionInMap.zPosition));
						leftFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition - relativePos, entity.PositionInMap.yPosition + relativePos, entity.PositionInMap.zPosition));
						rightFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition + relativePos, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					}
					break;
				case WorldAxis.PositiveZ:
					break;
				case WorldAxis.NegativeX:
					for (int blockCnt = 0; blockCnt <= relativeSight; blockCnt++)
					{
						int relativePos = 1 + blockCnt;
						fwdFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition - relativePos, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
						rearFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition + relativePos, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
						leftFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - relativePos, entity.PositionInMap.zPosition));
						rightFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + relativePos, entity.PositionInMap.zPosition));
					}
					break;
				case WorldAxis.NegativeY:
					for (int blockCnt = 0; blockCnt <= relativeSight; blockCnt++)
					{
						int relativePos = 1 + blockCnt;
						fwdFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition - relativePos, entity.PositionInMap.zPosition));
						rearFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition, entity.PositionInMap.yPosition + relativePos, entity.PositionInMap.zPosition));
						leftFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition + relativePos, entity.PositionInMap.yPosition + relativePos, entity.PositionInMap.zPosition));
						rightFacingBlocks.Add(GetBlockTypeAtMapPosition(entity.PositionInMap.xPosition - relativePos, entity.PositionInMap.yPosition, entity.PositionInMap.zPosition));
					}
					break;
				case WorldAxis.NegativeZ:
					break;
			}

			var posContext = new PositionContext(currentPosition, fwdFacingBlocks.ToArray(), rearFacingBlocks.ToArray(), leftFacingBlocks.ToArray(), rightFacingBlocks.ToArray());
			return posContext;
		}

		private MapBlock GetBlockTypeAtMapPosition(int xPos, int yPos, int zPos)
		{
			var blockInMap = _world.WorldMap.Get(xPos, yPos, zPos);
			var entities= _world.Inhabitants.ToArray();
			for (int cnt=0; cnt < entities.Length; cnt++)
			{
				var entity = entities[cnt];
				if (entity.PositionInMap.xPosition == xPos && entity.PositionInMap.yPosition == yPos && entity.PositionInMap.zPosition == zPos)
				{
					blockInMap = new PlayerOccupiedBlock();
				}
			}
			return blockInMap;
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
