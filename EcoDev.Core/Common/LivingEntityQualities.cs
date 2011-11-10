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
				return GetEvenlyDistributedRangeOfAdditivesForQualities(Speed);
			}
		}

		public int RelativeSight
		{
			get
			{
				return GetEvenlyDistributedRangeOfAdditivesForQualities(Sight);
			}
		}

		private int GetEvenlyDistributedRangeOfAdditivesForQualities(byte qualityValue)
		{
			if (qualityValue == 0)
				return 0;
			return (int)(qualityValue / (byte.MaxValue / 3)) + 1;
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
