using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.MapEngine;
using EcoDev.Engine.Entities;
using System.Threading.Tasks;
using System.Threading;
using EcoDev.Engine.WorldEngine;
using EcoDev.Core.Common.BuildingBlocks;

namespace EcoDev.Visualisation.Basic
{
	public class VisualisationEngine
	{
		private Dictionary<Guid, LivingEntityWithQualities> _currentPlayers = new Dictionary<Guid, LivingEntityWithQualities>();
		public Dictionary<Guid,LivingEntityWithQualities> CurrentPlayers { get { return _currentPlayers; } }
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();
		private EcoWorld _world;
		ConsoleColor _originalConsoleColor;
		ConsoleColor _edgeColor = ConsoleColor.Black;
		ConsoleColor _playerColor = ConsoleColor.Yellow;
		ConsoleColor _solidBlockColor = ConsoleColor.DarkRed;
		ConsoleColor _entranceColor = ConsoleColor.Green;
		ConsoleColor _exitColor = ConsoleColor.Magenta;

		public VisualisationEngine(EcoWorld world)
		{
			_world = world;
		}
		
		public void StartMapRendering()
		{
			Task.Factory.StartNew(() => 
			{
				Console.Clear();

				while (!_tokenSource.IsCancellationRequested)
				{
					DrawMap();
				}
			}, _tokenSource.Token);
		}

		public void StopMapRendering()
		{
			_tokenSource.Cancel();
		}

		private void DrawMap()
		{
			_originalConsoleColor = Console.ForegroundColor;
			Console.CursorLeft = 0;
			Console.CursorTop = 0;

			for (int zPos = 0; zPos < _world.WorldMap.DepthInUnits; zPos++)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("{1}Level: {0}{1}{1}", zPos, Environment.NewLine);

				// Draw the boundary
				for (int xPos = -1; xPos <= _world.WorldMap.WidthInUnits; xPos++)
				{
					Console.ForegroundColor = _edgeColor;
					Console.Write("_");
				}
				Console.Write("{0}", Environment.NewLine);

				//do y position in reverse so the text rendering starts at the top and works down the graph
				for (int yPos = _world.WorldMap.HeightInUnits - 1; yPos >= 0; yPos--)
				{
					Console.ForegroundColor = _edgeColor;
					Console.Write("|");
					for (int xPos = 0; xPos < _world.WorldMap.WidthInUnits; xPos++)
					{
						var textBlock = _world.WorldMap.Get(xPos, yPos, zPos);
						if (IsPlayerAtThisPosition(xPos,yPos,zPos))
						{
							Console.ForegroundColor = _playerColor;
							Console.Write("*");
						}
						else
						{
							if (textBlock is MapExitBlock)
							{
								Console.ForegroundColor = _exitColor;
							}
							else if (textBlock is MapEntranceBlock)
							{
								Console.ForegroundColor = _entranceColor;
							}
							else
							{
								Console.ForegroundColor = _solidBlockColor;
							}

							Console.Write(textBlock != null ? textBlock.ToString() : " ");
						}
						
					}
					Console.ForegroundColor = _edgeColor;
					Console.Write("|{0}", Environment.NewLine);
				}

				// Draw the boundary
				for (int xPos = -1; xPos <= _world.WorldMap.WidthInUnits; xPos++)
				{
					Console.ForegroundColor = _edgeColor;
					Console.Write("_");
				}
				Console.Write("{0}", Environment.NewLine);
			}

			Console.ForegroundColor = _originalConsoleColor;
		}

		private bool IsPlayerAtThisPosition(int xPos, int yPos, int zPos)
		{
			var players = _world.Inhabitants.ToList();
			for (int cnt = 0; cnt < players.Count; cnt++ )
			{
				var entity = players[cnt];
				if (entity.PositionInMap.xPosition == xPos && entity.PositionInMap.yPosition == yPos && entity.PositionInMap.zPosition == zPos)
				{
					return true;
				}
			}

			return false;
		}
	}
}
