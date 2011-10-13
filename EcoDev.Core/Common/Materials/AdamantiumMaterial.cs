using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.Materials
{
	public class AdamantiumMaterial: EntityMaterial
	{
		public AdamantiumMaterial()
		{
			Name = "Adamantium";
			Strength = double.MaxValue;
		}
	}
}
