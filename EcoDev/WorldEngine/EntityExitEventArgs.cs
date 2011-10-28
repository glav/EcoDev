using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.Entities;

namespace EcoDev.Engine.WorldEngine
{
	public class EntityExitEventArgs : EventArgs
	{
		public EntityExitEventArgs(LivingEntityWithQualities inhabitant)
		{
			Inhabitant = inhabitant;
		}
		public LivingEntityWithQualities Inhabitant {get; set;}
	}
}
