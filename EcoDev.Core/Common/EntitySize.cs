using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common
{
	public class EntitySize
	{
		public double Width { get; set; }
		public double Height { get; set; }
		public double Thickness { get; set; }

		public override string ToString()
		{
			StringBuilder props = new StringBuilder();
			props.AppendFormat("EntitySize: Width [{0}], Height: [{1}], Thickness: [{2}]", Width, Height, Thickness);
			return props.ToString();

		}
	}
}
