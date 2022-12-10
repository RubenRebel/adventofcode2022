using System;
namespace AdventofCode2022.Solutions
{
    public class Day9 : SolutionBase
    {
        List<BridgePosition> visitedPositions = new List<BridgePosition>();

        public Day9(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var planksInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/planks.txt");
            var directions = stringUtils.SplitStringsOnNewLines(planksInput);
            PartOne(directions);
            PartTwo(directions);
        }

        private void PartTwo(List<string> directions)
        {
            visitedPositions = new List<BridgePosition>();
            visitedPositions.Add(new BridgePosition());
            var head = new BridgePosition();
            var tails = new List<BridgePosition>();
            for (int i = 0; i < 9; i++)
            {
                tails.Add(new BridgePosition());
            }

            foreach (var line in directions)
            {
                var split = line.Split(' ');
                var direction = split[0];
                var distance = int.Parse(split[1].ToString());

                for (int i = 0; i < distance; i++)
                {
                    MoveHead(direction, head);
                    var newHead = head;
                    for (int y = 0; y < tails.Count; y++)
                    {
                        MoveTail(tails[y], newHead);
                        newHead = tails[y];
                    }
                    CheckIfVisited(tails[tails.Count-1]);
                }
            }

            LogPuzzleInformation(9, "Rope Bridge part two");
            LogPuzzleAnswer(visitedPositions.Count().ToString(), "Rope Bridge part two");
        }

        private void PartOne(List<string> directions)
        {
            visitedPositions.Add(new BridgePosition());
            var head = new BridgePosition();
            var tail = new BridgePosition();
            foreach (var line in directions)
            {
                var split = line.Split(' ');
                var direction = split[0];
                var distance = int.Parse(split[1].ToString());

                for (int i = 0; i < distance; i++)
                {
                    MoveHead(direction, head);
                    MoveTail(tail, head);
                    CheckIfVisited(tail);
                }
            }

            LogPuzzleInformation(9, "Rope Bridge part one");
            LogPuzzleAnswer(visitedPositions.Count().ToString(), "Rope Bridge part one");
        }

        private void CheckIfVisited(BridgePosition tail)
        {
            if (visitedPositions.Where(p => p.xPos == tail.xPos && p.yPos == tail.yPos).Count() == 0)
            {
                var newPosition = new BridgePosition();
                newPosition.xPos = tail.xPos;
                newPosition.yPos = tail.yPos;
                visitedPositions.Add(newPosition);
            }
        }

        private void MoveTail(BridgePosition tail, BridgePosition head)
        {   
                var yDif = head.yPos - tail.yPos;

                if (yDif < -1)
                {
                    tail.yPos--;
                    if (head.xPos > tail.xPos)
                    {
                        tail.xPos++;
                    }
                    if (head.xPos < tail.xPos)
                    {
                        tail.xPos--;
                    }
                }

                if (yDif > 1)
                {
                    tail.yPos++;
                    if (head.xPos > tail.xPos)
                    {
                        tail.xPos++;
                    }
                    if (head.xPos < tail.xPos)
                    {
                        tail.xPos--;
                    }
                }
            
                var xDif = head.xPos - tail.xPos;

                if (xDif > 1)
                {
                    tail.xPos++;
                    if (tail.yPos != head.yPos)
                    {
                        tail.yPos = head.yPos;
                    }
                }

                if (xDif < -1)
                {
                    tail.xPos--;
                    if (tail.yPos != head.yPos)
                    {
                        tail.yPos = head.yPos;
                    }
                }
        }

        private void MoveHead(string direction, BridgePosition head)
        {
            switch (direction)
            {
                case "R":
                    head.xPos++;
                    break;
                case "L":
                    head.xPos--;
                    break;
                case "U":
                    head.yPos--;
                    break;
                case "D":
                    head.yPos++;
                    break;
                default:
                    break;
            }
        }
    }

    internal class BridgePosition
    {
        public int xPos = 0;
        public int yPos = 0;
    }
}