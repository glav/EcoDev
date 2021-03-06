﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.Entities;
using EcoDev.Engine.MapEngine;
using EcoDev.Core.Common.Actions;

namespace EcoDev.Engine.WorldEngine
{
	internal abstract class ActionResponse
	{
		private IEcoWorld _world;
		public ActionResponse(IEcoWorld world)
		{
			_world = world;
		}
		public LivingEntityWithQualities Inhabitant { get; set; }
		public IEcoWorld World { get { return _world; } }
		public ActionResult DecidedAction { get; set; }

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
