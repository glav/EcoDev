using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common.Actions
{
	public class ActionContext
	{
		public ActionContext(PositionContext position)
		{
			Position = position;
		}
		public PositionContext Position { get; private set; }
	}

	public class PositionContext
	{
		public PositionContext(MapBlock currentPosition, MapBlock[] forwardFacingPositions, MapBlock[] rearFacingPositions, 
								MapBlock[] leftFacingPositions, MapBlock[] rightFacingPositions)
		{
			CurrentPosition = currentPosition;
			ForwardFacingPositions = forwardFacingPositions;
			RearFacingPositions = rearFacingPositions;
			LeftFacingPositions = leftFacingPositions;
			RightFacingPositions = rightFacingPositions;
		}
		public MapBlock CurrentPosition { get; private set; }
		public MapBlock[] ForwardFacingPositions { get; private set; }
		public MapBlock[] RearFacingPositions { get; private set; }
		public MapBlock[] LeftFacingPositions { get; private set; }
		public MapBlock[] RightFacingPositions { get; private set; }
	}
}
