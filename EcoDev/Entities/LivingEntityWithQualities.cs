using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common;
using EcoDev.Engine.WorldEngine;
using EcoDev.Core.Common.Maps;

namespace EcoDev.Engine.Entities
{
	public class LivingEntityWithQualities
	{
		public LivingEntityWithQualities()
		{
			Qualities = new LivingEntityQualities();
			PositionInMap = new MapPosition();
		}
		public LivingEntity Entity { get; set; }
		public LivingEntityQualities Qualities { get; set; }
		public MapPosition PositionInMap { get; set; }
		public WorldAxis ForwardFacingAxis { get; set; }
	}
}
