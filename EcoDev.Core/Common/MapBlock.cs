using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.BuildingBlocks;

namespace EcoDev.Core.Common
{
	public abstract class MapBlock: BaseEntity
	{
		public bool IsUnmoveable { get; set; }
		public abstract MapBlockAccessibility Accessibility { get; }

		public override EntityBaseType EntityType
		{
			get { return EntityBaseType.BuildingBlock; }
		}
	}
}
