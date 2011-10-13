using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.Materials
{
	public abstract class EntityMaterial
	{
		public string Name { get; set; }
		public double Strength { get; set; } // 0 == immediately breakable, double.Max == unbreakable
		
		public override string ToString()
		{
			return string.Format("EntityMaterial: Name: [{0}], Strength: [{1}]", Name, Strength);
		}
	}
}
