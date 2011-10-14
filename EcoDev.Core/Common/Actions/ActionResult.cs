using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.Actions
{
	public abstract class ActionResult
	{
		public ActionToPerform DecidedAction { get; set; }
		public MovementDirection DirectionToMove { get; set; }
	}
}
