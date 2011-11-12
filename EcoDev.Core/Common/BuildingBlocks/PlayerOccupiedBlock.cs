using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.Maps;

namespace EcoDev.Core.Common.BuildingBlocks
{
	public class PlayerOccupiedBlock : MapBlock
	{
		public override MapBlockAccessibility Accessibility
		{
			get { return MapBlockAccessibility.CannotGainEntryOrExit; }
		}
	}
}
