using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common;
using EcoDev.Core.Common.Maps;

namespace EcoDev.Core.Common.BuildingBlocks
{
	public class MapExitBlock : MapBlock
	{
		public MapExitBlock()
		{
			IsUnmoveable = true;
		}

		public override MapBlockAccessibility Accessibility
		{
			get { return MapBlockAccessibility.AllowExit; }
		}

		public override string ToString()
		{
			return "^";
		}
	}
}
