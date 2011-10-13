using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core;
using EcoDev.Core.Common;
using EcoDev.Core.Common.Materials;

namespace EcoDev.Core.Common.BuildingBlocks
{
	public class SolidBlock : MapBlock
	{
		public SolidBlock()
		{
			LifeKey = new Guid("B0F05F64-0685-4480-A6AD-26D0B72DDBC1");
			Name = "Solid Block";
			IsUnmoveable = true;
			CompositionMaterial = new AdamantiumMaterial();
			Size = new EntitySize() { Height = double.MaxValue, Thickness = double.MaxValue, Width = double.MaxValue };
			Weight = double.MaxValue;
		}

		public override MapBlockAccessibility Accessibility
		{
			get { return MapBlockAccessibility.CannotGainEntryOrExit; }
		}

		public override string ToString()
		{
			return "#";
		}
	}
}
