using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.Maps
{
	public class MapPosition
	{
		public MapPosition() { }

		public MapPosition(int x, int y, int z)
		{
			xPosition = x;
			yPosition = y;
			zPosition = z;
		}

		public int xPosition { get; set; }
		public int yPosition { get; set; }
		public int zPosition { get; set; }

		public WorldAxis DetermineForwardFacingPositionBasedOnThisPosition(int mapWidthInUnits, int mapHeightInUnits, int mapDepthInUnits)
		{
			WorldAxis fwdFacingAxis = WorldAxis.PositiveX;

			if (xPosition == 0)
			{
				return WorldAxis.PositiveX;
			}
			if (xPosition == mapWidthInUnits - 1)
			{
				return WorldAxis.NegativeX;
			}
			if (yPosition == 0)
			{
				return WorldAxis.PositiveY;
			}
			if (yPosition == mapHeightInUnits - 1)
			{
				return WorldAxis.NegativeY;
			}

			if (zPosition == 0)
			{
				return WorldAxis.PositiveZ;
			}
			if (zPosition == mapDepthInUnits - 1)
			{
				return WorldAxis.NegativeZ;
			}
			return WorldAxis.PositiveX;
		}
	}
}
