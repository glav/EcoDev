using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.BuildingBlocks
{
	public enum MapBlockAccessibility
	{
		CannotGainEntryOrExit,
		AllowEntry,
		AllowExit,
		AllowPotentialEntry,
		AllowPotentialExit
	}
}
