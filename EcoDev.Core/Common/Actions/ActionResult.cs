using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.Actions
{
	public class ActionResult
	{
		public ActionResult()
		{
			DecidedAction = ActionToPerform.Nothing;
			DirectionToMove = MovementDirection.None;
		}
		public ActionToPerform DecidedAction { get; set; }
		public MovementDirection DirectionToMove { get; set; }
	}
}
