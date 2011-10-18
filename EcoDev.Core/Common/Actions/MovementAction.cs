using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.Actions
{
	public class MovementAction : ActionResult
	{
		public MovementAction()
		{
			DecidedAction = ActionToPerform.Move;
			DirectionToMove = MovementDirection.Forward;
		}
	}
}
