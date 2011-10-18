using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.Maps;
using EcoDev.Core.Common;

namespace EcoDev.Engine.MapEngine
{
	public class WorldMapBlock
	{
		public MapBlock MapBlockItem { get; set; }
		public BaseEntity EntityOccupyingBlockPosition { get; set; }
	}
}
