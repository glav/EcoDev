using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common;
using EcoDev.Core.Common.Maps;
using EcoDev.Core.Common.Actions;

namespace EcoDev.Engine.WorldEngine
{
	public class InhabitantActionEventArgs : EventArgs
	{
		public InhabitantActionEventArgs(ActionToPerform actionPerformed, MovementDirection directionMoved, MapPosition positionInMap)
		{
			ActionPerformed = actionPerformed;
			DirectionMoved = directionMoved;
			PositionInMap = positionInMap;
		}
		public ActionToPerform ActionPerformed { get; set; }
		public MovementDirection DirectionMoved { get; set; }
		public MapPosition PositionInMap { get; set; }
	}
}
