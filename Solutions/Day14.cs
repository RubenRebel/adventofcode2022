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

        List<Tile> Cave;
        List<List<Point>> RockPaths;

        public Day14(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var regolithInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/regolith.txt");
            var split = stringUtils.SplitStringsOnNewLines(regolithInput);
            PrepareCave(split);
            //FlowSand();
            DrawCave();
            // to prepare map
            // keep track of 2 lists:
            //  list<list<Point>> of rockPaths containing points of rock coordinates
            //  list<Tile> of cave containing Tile elements
            // for each line in regolitInput add the coordinates of the path to the rockPaths
            // get the coordinate min and max range of the point list
            // Fill cave list with all coordinates between this range and:
            // set Value to rock (#) if the coordinate is in rockPaths or set Value to + if coordinate is 500,0. Else Value is air

            // to make sand fall
            // start at 500,0
            // keep track of current position
            // If  y + 1 and x is air. if so adjust current position
            // else if y + 1 and x - 1 is air. if so adjust current position
            // else if y + 1 and x + 1 is air. if so adjust current position
            // else sand cannot move any further. Draw 0 in current position and increase unit of sand counter
            // repeat above process until:
            // current position x or y exceeds map range, meaning sand falls into the void. Print sand counter which is answer for puzzle
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

            // second get cave grid
            var startX = RockPaths.SelectMany(m => m).Min(p => p.X);
            var endX = RockPaths.SelectMany(m => m).Max(p => p.X);
            var endY = RockPaths.SelectMany(m => m).Max(p => p.Y);
            for (int y = 0; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    var point = new Point(x, y);
                    var tile = new Tile();
                    tile.Location = point;
                    if (x == 500 && y == 0)
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

        }

        private void DrawCave()
        {
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

