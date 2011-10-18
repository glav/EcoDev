using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.Materials;

namespace EcoDev.Core.Common
{
	public abstract class BaseEntity
	{
		public string Name { get; set; }
		public Guid LifeKey { get; set; }
		public abstract EntityBaseType EntityType { get; }
		public IWorld World { get; set; }

		public EntitySize Size { get; set; }
		public EntityMaterial CompositionMaterial { get; set; }
		public double Weight { get; set; }

		public override string ToString()
		{
			StringBuilder props = new StringBuilder();
			props.AppendFormat("BaseEntity:\n\tLifeKey: [{0}]\n\tName: [{1}]",LifeKey, Name);
			props.AppendFormat("\n\tEntityType: [{0}]",SafeToString(EntityType));
			props.AppendFormat("\n\tSize: [{0}]", SafeToString(Size));
			props.AppendFormat("\n\tCompositionMaterial: [{0}]",SafeToString(CompositionMaterial));
			props.AppendFormat("\n\tWeight:[{0}]", Weight);
			return props.ToString();
		}

		protected string SafeToString(object property)
		{
			return property != null ? property.ToString() : string.Empty;
		}
	}
}
