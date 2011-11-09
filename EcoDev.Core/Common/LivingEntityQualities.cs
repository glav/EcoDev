using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common
{
	public class LivingEntityQualities
	{
		public byte Intelligence { get; set; }
		public byte Sight { get; set; }
		public byte Hearing { get; set; }
		public byte Touch { get; set; }
		public byte Intuition { get; set; }
		public byte Stamina { get; set; }
		public byte Speed { get; set; } // Min 1, max 255 - 3 levels (0-85 = move 1 block, 86-170 = move 2 space, 171-255 = move 3 spaces
		public byte Strength { get; set; }
		public byte Reflexes { get; set; }
		public byte Agility { get; set; }
		
		public int RelativeSpeed
		{
			get
			{
				return Math.Max((int)(Speed / (byte.MaxValue / 3)), 1);
			}
		}

		public int RelativeSight
		{
			get
			{
				return (int)(entity.Qualities.Sight / (byte.MaxValue / 3));
			}
		}

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
