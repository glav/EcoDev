using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoDev.Engine.MapEngine
{
	public class MapInvalidException : System.Exception
	{
		public MapInvalidException() : base() { } 
		public MapInvalidException(string message) : base(message) { }
		public MapInvalidException(string message, Exception innerException) : base(message, innerException) { }
	}
}
