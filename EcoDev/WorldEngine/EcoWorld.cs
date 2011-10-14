using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common;

namespace EcoDev.Engine.WorldEngine
{
	public class EcoWorld
	{
		Map _worldMap;
		List<LivingEntity> _inhabitants = new List<LivingEntity>();

		public EcoWorld(Map worldMap, LivingEntity[] inhabitants)
		{
			if (worldMap == null)
			{
				throw new ArgumentException("World Map cannot be NULL");
			}
			_worldMap = worldMap;

			if (inhabitants != null && inhabitants.Length > 0)
			{
				_inhabitants.AddRange(inhabitants);
			}
		}

		public void AddPlayer(PlayerEntity player)
		{
		}

		public void StartWorld()
		{
		}
	}
}
