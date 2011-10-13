using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common;
using EcoDev.Core.Common.BuildingBlocks;

namespace EcoDev.Engine.MapEngine
{
	public class Map
	{
		private MapBlock[, ,] _mapContainer;

		public Map(long width, long height, long depth)
		{
			if (width <= 0 || height <= 0 || depth <= 0)
			{
				throw new ArgumentOutOfRangeException("Dimensions of map cannot be less than or equal to 0");
			}

			_widthInUnits = width;
			_heightInUnits = height;
			_depthInUnits = depth;

			CreateMapContainer();
		}

		private void CreateMapContainer()
		{
			_mapContainer = new MapBlock[_widthInUnits, _heightInUnits, _depthInUnits];
		}

		private long _widthInUnits;
		public long WidthInUnits { get { return _widthInUnits; } }

		private long _heightInUnits;
		public long HeightInUnits { get { return _heightInUnits; } }

		private long _depthInUnits;
		public long DepthInUnits { get { return _depthInUnits; } }

		public void Set(long x, long y, long z, MapBlock block)
		{
			ValidatePosition(x, y, z);
			_mapContainer[x, y, z] = block;
		}

		protected void ValidatePosition(long x, long y, long z)
		{
			if (x > (_widthInUnits - 1)
				|| x < 0
				|| y > (_heightInUnits - 1)
				|| y < 0
				|| (z > _depthInUnits - 1)
				|| z < 0)
			{
				throw new ArgumentOutOfRangeException("Position arguments are out of range of map");
			}
		}

		public void CreateWorld()
		{
			ValidateMap();
		}

		protected void ValidateMap()
		{
			bool hasEntry = false;
			bool hasExit = false;

			for (long xPos = 0; xPos < _widthInUnits; xPos++)
			{
				for (long yPos = 0; yPos < _heightInUnits; yPos++)
				{
					for (int zPos = 0; zPos < _depthInUnits; zPos++)
					{
						var mapBlock = _mapContainer[xPos, yPos, zPos];
						if (mapBlock != null)
						{
							if (mapBlock.Accessibility == MapBlockAccessibility.AllowEntry)
							{
								hasEntry = true;
							}
							if (mapBlock.Accessibility == MapBlockAccessibility.AllowExit)
							{
								hasExit = true;
							}
						}

						if (hasExit && hasEntry)
						{
							break;
						}

					}
				}
			}

			if (!hasEntry || !hasExit)
			{
				throw new MapInvalidException("Map must have at least 1 entry and 1 exit");
			}
		}

		public override string ToString()
		{
			StringBuilder mapText = new StringBuilder();

			for (long zPos = 0; zPos < _depthInUnits; zPos++)
			{
				mapText.AppendFormat("{1}Level: {0}{1}{1}", zPos, Environment.NewLine);

				// Draw the boundary
				for (long xPos = -1; xPos <= _widthInUnits; xPos++)
				{
					mapText.Append("_");
				}
				mapText.AppendFormat("{0}", Environment.NewLine);

				//do y position in reverse so the text rendering starts at the top and works down the graph
				for (long yPos = _heightInUnits - 1; yPos >= 0; yPos--)
				{
					mapText.Append("|");
					for (int xPos = 0; xPos < _widthInUnits; xPos++)
					{
						var textBlock = ConvertTextBlockToMapBlock(_mapContainer[xPos, yPos, zPos]);
						mapText.Append(textBlock);
					}
					mapText.AppendFormat("|{0}", Environment.NewLine);
				}

				// Draw the boundary
				for (long xPos = -1; xPos <= _widthInUnits; xPos++)
				{
					mapText.Append("_");
				}
				mapText.AppendFormat("{0}", Environment.NewLine);
			}

			return mapText.ToString();
		}

		private string ConvertTextBlockToMapBlock(MapBlock mapBlock)
		{
			if (mapBlock == null)
			{
				return " ";
			}

			return mapBlock.ToString();

		}
	}
}
