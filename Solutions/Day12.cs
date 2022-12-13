using System;
using System.Drawing;
namespace AdventofCode2022.Solutions
{
    public class Day12 : SolutionBase
    {
        List<List<Mountain>> Grid;
        Point StartPosition;
        Point DestinationPosition;
        List<Mountain> OpenNodes;
        List<Mountain> ClosedNodes;
        Mountain CurrentNode;
        List<string> Split;

        public Day12(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var climbingInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/climbing.txt");
            Split = stringUtils.SplitStringsOnNewLines(climbingInput);

            PartOne();
            PartTwo();
        }

        private void PartTwo()
        {
            PrepareGrid(Split);
            var startPositions = new List<Point>();
            foreach (var column in Grid)
            {
                foreach (var row in column)
                {
                    if(row.Height == 1)
                    {
                        startPositions.Add(row.Location);
                    }
                }
            }

            var pathLengths = new List<int>();
            foreach (var startPos in startPositions)
            {
                StartPosition = startPos;
                GetPath();
                pathLengths.Add(GetCurrentPathLength());
                PrepareGrid(Split);
            }

            var lowest = pathLengths.Where(p => p != 0).Min(path => path);
            var shortestPath = pathLengths.Where(node => node == lowest).FirstOrDefault();

            LogPuzzleInformation(12, $"Hill Climbing Algorithm part two");
            LogPuzzleAnswer(shortestPath.ToString(), $"Hill Climbing Algorithm part two");
        }

        private void PartOne()
        {
            PrepareGrid(Split);
            GetPath();
            LogPuzzleInformation(12, $"Hill Climbing Algorithm part one");
            LogPuzzleAnswer(GetCurrentPathLength().ToString(), $"Hill Climbing Algorithm part one");
        }

        private void GetPath()
        {
            var mountain = Grid[StartPosition.X][StartPosition.Y];
            OpenNodes.Add(mountain);

            while (true)
            {
                if(OpenNodes.Count == 0)
                {
                    CurrentNode = null;
                    break;
                }
                var lowest = OpenNodes.Min(node => node.FCost);
                var nextMountain = OpenNodes.Where(node => node.FCost == lowest).ToList();

                // if node costs are same, pick the one with lowest H cost
                if (nextMountain.Count > 1)
                {
                    var lowestH = nextMountain.Min(node => node.HCost);

                    nextMountain = nextMountain.Where(node => node.HCost == lowestH).ToList();
                }
                CurrentNode = nextMountain.FirstOrDefault();
                OpenNodes.Remove(CurrentNode);
                ClosedNodes.Add(CurrentNode);

                if (CurrentNode.Location == DestinationPosition)
                {
                    break;
                }
                ReviewNeighbours();
            }
        }

        private void ReviewNeighbours()
        {
            var neighbours = new List<Mountain>();
            if(CurrentNode.Location.X + 1 < Grid.Count())
            {
                neighbours.Add(Grid[CurrentNode.Location.X + 1][CurrentNode.Location.Y]);
            }
            if (CurrentNode.Location.X - 1 >= 0)
            {
                neighbours.Add(Grid[CurrentNode.Location.X - 1][CurrentNode.Location.Y]);
            }
            if (CurrentNode.Location.Y + 1 < Grid[0].Count())
            {
                neighbours.Add(Grid[CurrentNode.Location.X][CurrentNode.Location.Y + 1]);
            }
            if (CurrentNode.Location.Y - 1 >= 0)
            {
                neighbours.Add(Grid[CurrentNode.Location.X][CurrentNode.Location.Y - 1]);
            }

            foreach (var neighbour in neighbours)
            {
                if(ClosedNodes.Contains(neighbour) || neighbour.Height > CurrentNode.Height + 1)
                {
                    continue;
                }

                var GCost = GetCurrentPathLength();

                if (GCost < neighbour.GCost || !OpenNodes.Contains(neighbour))
                {
                    var HCost = ReturnPositiveDifference(DestinationPosition.X, neighbour.Location.X, DestinationPosition.Y, neighbour.Location.Y);

                    neighbour.FCost = HCost + GCost;
                    neighbour.GCost = GCost;
                    neighbour.Parent = CurrentNode;                

                    if (!OpenNodes.Contains(neighbour))
                    {
                        OpenNodes.Add(neighbour);
                    }
                }    
            }
        }

        private int GetCurrentPathLength()
        {
            var steps = 0;
            var currentNode = CurrentNode;

            if (CurrentNode == null)
            {
                return steps;
            }
            
            while (currentNode.Parent != null)
            {
                steps++;
                currentNode = currentNode.Parent;
            }
            return steps;
        }

        private void PrepareGrid(List<string> split)
        {
            Grid = new List<List<Mountain>>();
            OpenNodes = new List<Mountain>();
            ClosedNodes = new List<Mountain>();

            for (int x = 0; x < split.Count(); x++)
            {
                for (int y = 0; y < split[x].Count(); y++)
                {
                    if (split[x][y] == 'S')
                    {
                        StartPosition = new Point(y, x);
                    }
                    if (split[x][y] == 'E')
                    {
                        DestinationPosition = new Point(y, x);
                    }
                }
            }

            for (int x = 0; x < split.Count(); x++)
            {
                for (int y = 0; y < split[x].Count(); y++)
                {
                    if (x == 0)
                    {
                        Grid.Add(new List<Mountain>());
                    }

                    var height = (int)split[x][y] % 32;
                    
                    if (split[x][y] == 'S')
                    {
                        height = 0;
                    }
                    if (split[x][y] == 'E')
                    {
                        height = 27;
                    }

                    var hCost = ReturnPositiveDifference(DestinationPosition.X, x, DestinationPosition.Y, y);
                    var gCost = ReturnPositiveDifference(StartPosition.X, x, StartPosition.Y, y);

                    var mountain = new Mountain()
                    {
                        Height = height,
                        Location = new Point(y, x),
                        HCost = hCost,
                        GCost = gCost,
                        FCost = hCost + gCost
                    };

                    Grid[y].Add(mountain);  
                }
            }
        }

        private int ReturnPositiveDifference(int xStart, int xEnd, int yStart, int yEnd)
        {
            var xDif = xStart - xEnd;
            if(xDif < 0)
            {
                xDif *= -1;
            }
            var yDif = yStart - yEnd;
            if (yDif < 0)
            {
                yDif *= -1;
            }
            return xDif + yDif;
        }
    }

    internal class Mountain
    {
        // distance from endnode
        public int HCost { get; set; }
        //// distance from startnode
        public int GCost { get; set; }
        // sum of HCost and GCost
        public int FCost { get; set; }
        public Mountain Parent { get; set; }
        public int Height { get; set; }
        public Point Location { get; set; }
    }
}