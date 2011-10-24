using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.Entities;

namespace EcoDev.Engine.WorldEngine
{
	internal abstract class ActionResponse
	{
		public LivingEntityWithQualities Entity { get; set; }
		protected abstract void HandleActionToPerform();
		public void ExecuteActionToPerform()
		{
			//TODO: Here we should execute the action in such a way to only
			// take a maximum of 500 milliseconds to run. This is prolly best done on a task
			// or thread. This should even cater for such dramatic cases where the implementor
			// of the player does a thread.sleep or something stupid so we dont hold up the
			// entire world processing based on 1 player.
			HandleActionToPerform();
		}
	}
}
