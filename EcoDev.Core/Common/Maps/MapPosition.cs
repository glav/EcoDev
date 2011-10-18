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
	}
}
