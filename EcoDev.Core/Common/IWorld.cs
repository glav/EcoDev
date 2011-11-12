using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Core.Common
{
	// This is the interface available to a player/inhabitant so ensure we dont allow anything too visible in the 
	// world to be available. It is their "window" on the world and is a subset of the
	// full world.
	public interface IInhabitantWorld
	{
		void WriteDebugInformation(string source, string message);
	}
}
