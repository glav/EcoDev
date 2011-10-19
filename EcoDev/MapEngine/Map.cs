using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common;
using EcoDev.Core.Common.BuildingBlocks;
using System.Threading.Tasks;
using EcoDev.Core.Common.Maps;

namespace EcoDev.Engine.MapEngine
{
	public class Map
	{
		private MapBlock[,,] _mapContainer;

		public Map(int width, int height, int depth)
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

		private List<MapPosition> _entrances = new List<MapPosition>();
		public IEnumerable<MapPosition> Entrances { get { return _entrances; } }

		private List<MapPosition> _exits = new List<MapPosition>();
		public IEnumerable<MapPosition> Exits { get { return _exits; } }

		private void CreateMapContainer()
		{
			_mapContainer = new MapBlock[_widthInUnits, _heightInUnits, _depthInUnits];
		}

		private int _widthInUnits;
		public int WidthInUnits { get { return _widthInUnits; } }

		private int _heightInUnits;
		public int HeightInUnits { get { return _heightInUnits; } }

		private int _depthInUnits;
		public int DepthInUnits { get { return _depthInUnits; } }

		public bool Set(int x, int y, int z, MapBlock block)
		{
			ValidatePosition(x, y, z);
			var proposedPosition = _mapContainer[x, y, z];

			// Check for allowed block positioning
			if (proposedPosition == null)
			{
				_mapContainer[x, y, z] = block;
				return true;
			}

			return false;
		}

		public MapBlock Get(int x, int y, int z)
		{
			// TODO: Check map dimensions before returning a block. If exceeds, return null
			return _mapContainer[x, y, z];
		}

		public void Clear(int x, int y, int z)
		{
			ValidatePosition(x,y,z);
			_mapContainer[x, y, z] = null;
		}

		public void ClearAll()
		{
			Parallel.For(0, _widthInUnits, (xPos) =>
			{
				for (int yPos = 0; yPos < _heightInUnits; yPos++)
				{
					for (int zPos = 0; zPos < _depthInUnits; zPos++)
					{
						_mapContainer[xPos, yPos, zPos] = null;
					}
				}
			});
		}

		protected void ValidatePosition(int x, int y, int z)
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

		public void InitialiseMap()
		{
			if (ValidateMap() == false)
			{
				throw new MapInvalidException("Map is invalid. Ensure at least 1 entrance and exit are positioned on the edge of the map");
			}
		}

		protected bool ValidateMap()
		{
			Parallel.For(0, _widthInUnits, (xPos) =>
			{
				for (int yPos = 0; yPos < _heightInUnits; yPos++)
				{
					for (int zPos = 0; zPos < _depthInUnits; zPos++)
					{
						var mapBlock = _mapContainer[xPos, yPos, zPos];
						if (mapBlock != null)
						{
							if (mapBlock.Accessibility == MapBlockAccessibility.AllowEntry)
							{
								if (IsPositionOnEdgeOfMap(xPos,yPos,zPos))
								{
									_entrances.Add(new MapPosition(xPos, yPos, zPos));
								}
							}
							if (mapBlock.Accessibility == MapBlockAccessibility.AllowExit)
							{
								if (IsPositionOnEdgeOfMap(xPos, yPos, zPos))
								{
									_exits.Add(new MapPosition(xPos, yPos, zPos));
								}
							}
						}
					}
				}
			});

			if (_entrances.Count == 0 || _exits.Count == 0)
			{
				return false;
			}

			return true;
		}

		private bool IsPositionOnEdgeOfMap(int x, int y, int z)
		{
			var xPosInValid = (x != 0 && x != (_widthInUnits-1));
			var yPosInvalid = (y != 0 && y != (_heightInUnits-1));
			var zPosInvalid = (z != 0 && z != (_depthInUnits-1));
			if (xPosInValid || yPosInvalid || zPosInvalid)
			{
				return false;
			}

			return true;
		}

		public override string ToString()
		{
			StringBuilder mapText = new StringBuilder();

			for (int zPos = 0; zPos < _depthInUnits; zPos++)
			{
				mapText.AppendFormat("{1}Level: {0}{1}{1}", zPos, Environment.NewLine);

				// Draw the boundary
				for (int xPos = -1; xPos <= _widthInUnits; xPos++)
				{
					mapText.Append("_");
				}
				mapText.AppendFormat("{0}", Environment.NewLine);

				//do y position in reverse so the text rendering starts at the top and works down the graph
				for (int yPos = _heightInUnits - 1; yPos >= 0; yPos--)
				{
					mapText.Append("|");
					for (int xPos = 0; xPos < _widthInUnits; xPos++)
					{
						var textBlock = ConvertMapBlockToTextBlock(_mapContainer[xPos, yPos, zPos]);
						mapText.Append(textBlock);
					}
					mapText.AppendFormat("|{0}", Environment.NewLine);
				}

				// Draw the boundary
				for (int xPos = -1; xPos <= _widthInUnits; xPos++)
				{
					mapText.Append("_");
				}
				mapText.AppendFormat("{0}", Environment.NewLine);
			}

			return mapText.ToString();
		}

		private string ConvertMapBlockToTextBlock(MapBlock mapBlock)
		{
			if (mapBlock == null)
			{
				return " ";
			}

			return mapBlock.ToString();

		}
	}
}
