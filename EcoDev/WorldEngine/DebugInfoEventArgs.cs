using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Engine.WorldEngine
{
	public class DebugInfoEventArgs : EventArgs
	{
		public DebugInfoEventArgs(string debugInformation)
		{
			DebugInformation = debugInformation;
		}

		public string DebugInformation { get; set; }
	}
}
