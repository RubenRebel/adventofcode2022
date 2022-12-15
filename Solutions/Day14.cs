using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace AdventofCode2022.Solutions
{
    public class Day14 : SolutionBase
    {
        char Rock = '#';
        char Air = '.';
        char Sand = 'o';
        char SandSource = '+';
        Point SandSourcePosition = new Point(500, 0);

        List<Tile> Cave;
        List<List<Point>> RockPaths;

        int MapStartX;
        int MapStartY = 0;
        int MapEndX;
        int MapEndY;
        int SandCounter = 0;

        public Day14(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var regolithInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/regolith.txt");
            var split = stringUtils.SplitStringsOnNewLines(regolithInput);

            // part one
            PrepareCave(split);
            FlowSand(false);
            LogPuzzleInformation(14, $"Regolith Reservoir part one");
            LogPuzzleAnswer(SandCounter.ToString(), $"Regolith Reservoir part part one");

            //part two
            SandCounter = 0;
            InsertFloor();
            FlowSand(true);
            LogPuzzleInformation(14, $"Regolith Reservoir part two");
            LogPuzzleAnswer(SandCounter.ToString(), $"Regolith Reservoir part part two");
        }
        private void PrepareCave(List<string> input)
        {
            Cave = new List<Tile>();
            RockPaths = new List<List<Point>>();

            // first get rock paths
            foreach (var line in input)
            {
                var coordinates = line.Split(" -> ");
                var path = new List<Point>();

                var baseCoordinate = coordinates[0].Split(',');
                var basePoint = new Point(int.Parse(baseCoordinate[0]), int.Parse(baseCoordinate[1]));

                for (int i = 1; i < coordinates.Count(); i++)
                {
                    var coordinate = coordinates[i].Split(',');
                    var currentPoint = new Point(int.Parse(coordinate[0]), int.Parse(coordinate[1]));
                    if (currentPoint.X != basePoint.X)
                    {
                        Point start;
                        Point end;
                        start = currentPoint.X > basePoint.X ? basePoint : currentPoint;
                        end = currentPoint.X > basePoint.X ? currentPoint : basePoint;
                        for (int x = start.X; x <= end.X; x++)
                        {
                            var newPoint = new Point(x, start.Y);
                            if (!path.Contains(newPoint))
                            {
                                path.Add(newPoint);
                            }
                        }
                    }
                    if (currentPoint.Y != basePoint.Y)
                    {
                        Point start;
                        Point end;
                        start = currentPoint.Y > basePoint.Y ? basePoint : currentPoint;
                        end = currentPoint.Y > basePoint.Y ? currentPoint : basePoint;
                        for (int y = start.Y; y <= end.Y; y++)
                        {
                            var newPoint = new Point(start.X, y);
                            if (!path.Contains(newPoint))
                            {
                                path.Add(newPoint);
                            }
                        }
                    }
                    basePoint = currentPoint;
                }
                RockPaths.Add(path);
            }

            // second make cave grid
            // here I add a column to the right and left (-1 and +1). I need this column
            // to check if the sand falls into it
            MapStartX = RockPaths.SelectMany(m => m).Min(p => p.X) - 1;
            MapEndX = RockPaths.SelectMany(m => m).Max(p => p.X) + 1;
            MapEndY = RockPaths.SelectMany(m => m).Max(p => p.Y);
            for (int y = MapStartY; y <= MapEndY; y++)
            {
                for (int x = MapStartX; x <= MapEndX; x++)
                {
                    var point = new Point(x, y);
                    var tile = new Tile();
                    tile.Location = point;
                    if (x == SandSourcePosition.X && y == SandSourcePosition.Y)
                    {
                        tile.Value = SandSource;
                    }
                    else if (RockPaths.SelectMany(m => m).Contains(point))
                    {
                        tile.Value = Rock;
                    }
                    else
                    {
                        tile.Value = Air;
                    }
                    Cave.Add(tile);
                }
            }
        }

        private void FlowSand(bool partTwo)
        {
            var currentPosition = SandSourcePosition;

            while (true)
            {
                var down = Cave.Where(tile => tile.Location.X == currentPosition.X && tile.Location.Y == currentPosition.Y + 1).FirstOrDefault();
                if (down != null && down.Value == Air)
                {
                    currentPosition = down.Location;
                    continue;
                }

                var downLeft = Cave.Where(tile => tile.Location.X == currentPosition.X - 1 && tile.Location.Y == currentPosition.Y + 1).FirstOrDefault();
                if (downLeft != null && downLeft.Value == Air)
                {
                    currentPosition = downLeft.Location;
                    continue;
                }

                var downRight = Cave.Where(tile => tile.Location.X == currentPosition.X + 1 && tile.Location.Y == currentPosition.Y + 1).FirstOrDefault();
                if (downRight != null && downRight.Value == Air)
                {
                    currentPosition = downRight.Location;
                    continue;
                }

                // sand cannot move any further.
                var currentTile = Cave.Where(tile => tile.Location.X == currentPosition.X  && tile.Location.Y == currentPosition.Y).First();
               
                if (currentPosition.X == MapStartX || currentPosition.X == MapEndX)
                {
                    if (!partTwo)
                    {
                        break;
                    }
                    ExtendFloor();
                }

                currentTile.Value = Sand;
                SandCounter++;
                if (partTwo && currentPosition == SandSourcePosition)
                {
                    break;
                }
                currentPosition = SandSourcePosition;
            }          
        }

        private void InsertFloor()
        {
            for (int x = MapStartX; x <= MapEndX; x++)
            {
                var newFloor = new Tile()
                {
                    Location = new Point(x, MapEndY + 1),
                    Value = Air
                };
                Cave.Add(newFloor);
            }
            MapEndY += 1;

            for (int x = MapStartX; x <= MapEndX; x++)
            {
                var newFloor = new Tile()
                {
                    Location = new Point(x, MapEndY + 1),
                    Value = Rock
                };
                Cave.Add(newFloor);
            }
            MapEndY += 1;
        }

        private void ExtendFloor()
        {
            // extend left
            for (int i = MapStartY; i <= MapEndY; i++)
            {
                var floorStart = Cave.FindIndex(tile => tile.Location.X == MapStartX && tile.Location.Y == i);

                var newFloorStart = new Tile()
                {
                    Location = new Point(MapStartX - 1, i),
                    Value = i == MapEndY ? Rock : Air
                };
                Cave.Insert(floorStart, newFloorStart);
            }

            // extend right
            for (int i = MapStartY; i <= MapEndY; i++)
            {
                var floorEnd = Cave.FindIndex(tile => tile.Location.X == MapEndX && tile.Location.Y == i);
                var newFloorEnd = new Tile()
                {
                    Location = new Point(MapEndX + 1, i),
                    Value = i == MapEndY ? Rock : Air
                };
                Cave.Insert(floorEnd + 1, newFloorEnd);
            }

            MapStartX -= 1;
            MapEndX += 1;
        }

        private void DrawCave()
        {
            Console.Clear();
            var index = 0;
            for (int y = MapStartY; y <= MapEndY; y++)
            {
                for (int x = MapStartX; x <= MapEndX; x++)
                {
                    Console.Write(Cave[index].Value);
                    index++;
                }
                Console.WriteLine();
            }
        }
    }

    internal class Tile
    {
        public Point Location { get; set; }
        public char Value { get; set; }
    }
}