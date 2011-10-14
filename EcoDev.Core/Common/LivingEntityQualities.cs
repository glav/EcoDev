using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common
{
	public abstract class LivingEntityQualities
	{
		public double Intelligence { get; set; }
		public double Sight { get; set; }
		public double Hearing { get; set; }
		public double Touch { get; set; }
		public double Intuition { get; set; }
		public double Stamina { get; set; }
		public double Speed { get; set; }
		public double Strength { get; set; }
		public double Reflexes { get; set; }
		public double Agility { get; set; }

		protected void SetQualities(LivingEntityQualities entityQualities)
		{
			Intelligence = entityQualities.Intelligence;
			Sight = entityQualities.Sight;
			Hearing = entityQualities.Hearing;
			Touch = entityQualities.Touch;
			Intuition = entityQualities.Intuition;
			Stamina = entityQualities.Stamina;
			Speed = entityQualities.Speed;
			Strength = entityQualities.Strength;
			Reflexes = entityQualities.Reflexes;
			Agility = entityQualities.Agility;
		}

		protected void UpdateQualitities(LivingEntityQualities entityQualities)
		{
			Intelligence += entityQualities.Intelligence;
			Sight += entityQualities.Sight;
			Hearing += entityQualities.Hearing;
			Touch += entityQualities.Touch;
			Intuition += entityQualities.Intuition;
			Stamina += entityQualities.Stamina;
			Speed += entityQualities.Speed;
			Strength += entityQualities.Strength;
			Reflexes += entityQualities.Reflexes;
			Agility += entityQualities.Agility;
		}
	}
}
