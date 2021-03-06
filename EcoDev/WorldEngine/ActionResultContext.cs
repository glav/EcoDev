﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.Actions;

namespace EcoDev.Engine.WorldEngine
{
	public class ActionResultContext
	{
		public ActionResultContext()
		{
			ActionResult = new ActionResult();
		}
		public ActionResultContext(ActionResult actionResult)
		{
			ActionResult = new ActionResult();
			ActionResult.DecidedAction = actionResult.DecidedAction;
			ActionResult.DirectionToMove = actionResult.DirectionToMove;
		}
		public ActionResult ActionResult { get; set; }
		public Exception ErrorException { get; set; }
	}
}
