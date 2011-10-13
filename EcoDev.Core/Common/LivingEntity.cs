using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common
{
	public abstract class LivingEntity : BaseEntity
	{
		public double Intelligence { get; set; }
		public double Sight { get; set; }
		public double Hearing { get; set; }
		public double Touch { get; set; }
		public double Intuition { get; set; }
		public double Stamina { get; set; }
		public double Speed { get; set; }
		public double Strength { get; set; }
		public double Reflex { get; set; }
		public double Agility { get; set; }

		public override EntityBaseType EntityType
		{
			get { return EntityBaseType.LivingEntity; }
		}
	}
}
