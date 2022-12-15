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

        public Day14(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var regolithInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/regolith.txt");
            var split = stringUtils.SplitStringsOnNewLines(regolithInput);
            PrepareCave(split);
            FlowSand();
        }

        private void PrepareCave(List<string> input)
        {
            // to prepare map
            // keep track of 2 lists:
            //  list<list<Point>> of rockPaths containing points of rock coordinates
            //  list<Tile> of cave containing Tile elements
            // for each line in regolitInput add the coordinates of the path to the rockPaths
            // get the coordinate min and max range of the point list
            // Fill cave list with all coordinates between this range and:
            // set Value to rock (#) if the coordinate is in rockPaths or set Value to + if coordinate is 500,0. Else Value is air

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

        private void FlowSand()
        {
            // start at 500,0
            // keep track of current position
            // If  y + 1 and x is air. if so adjust current position
            // else if y + 1 and x - 1 is air. if so adjust current position
            // else if y + 1 and x + 1 is air. if so adjust current position
            // else sand cannot move any further. Draw 0 in current position and increase unit of sand counter
            // repeat above process until:
            // if the x position of current is in either of the map's edges sand falls into the void. Print sand counter which is answer for puzzle
            var sandCounter = 0;
            var currentPosition = SandSourcePosition;
            var dumb = 0;
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
                    break;
                }

                currentPosition = SandSourcePosition;
                currentTile.Value = Sand;
                sandCounter++;
            }
            DrawCave();

            LogPuzzleInformation(14, $"Regolith Reservoir part one");
            LogPuzzleAnswer(sandCounter.ToString(), $"Regolith Reservoir part part one");
        }

        private void DrawCave()
        {
            Console.Clear();
            var start = Cave[0].Location;
            var end = Cave[Cave.Count() - 1].Location;
            var index = 0;
            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int x = start.X; x <= end.X; x++)
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