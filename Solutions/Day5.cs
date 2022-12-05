using System;
namespace AdventofCode2022.Solutions
{
    public class Day5 : SolutionBase
    {
        public Day5(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var rearrangeSheet = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/crates.txt");
            var split = stringUtils.SplitStringsOnNewLines(rearrangeSheet);

            var cratesConfiguration = new List<string>();
            var moves = new List<string>();
            var floorConfigLength = (split[0].Length + 1) / 4;
            var floorConfiguration = new List<Stack<char>>();

            for (int i = 0; i < floorConfigLength; i++)
            {
                floorConfiguration.Add(new Stack<char>());
            }
            var startOfMovesIndex = 0;

            // first order input data in stacks
            for (int i = 0; i < split.Count; i++)
            {
                if (string.IsNullOrEmpty(split[i]))
                {
                    startOfMovesIndex = i + 1;
                    break;
                }
                cratesConfiguration.Add(split[i]);            
            }

            var spot = cratesConfiguration.LastOrDefault();

            for (int i = cratesConfiguration.Count - 2; i >= 0; i--)
            {
                for (int y = 0; y < cratesConfiguration[i].Length; y++)
                {
                    if (cratesConfiguration[i][y] != ' ' && cratesConfiguration[i][y] != '[' && cratesConfiguration[i][y] != ']')
                    {
                        var crateIndex = int.Parse(spot[y].ToString());
                        floorConfiguration[crateIndex - 1].Push(cratesConfiguration[i][y]);
                    }
                }
            }

            // now apply move logic
            for (int i = startOfMovesIndex; i < split.Count; i++)
            {
                var indexMove = split[i].IndexOf("move ");
                var indexFrom = split[i].IndexOf("from ");
                var indexTo = split[i].IndexOf("to ");

                var move = int.Parse(split[i].Substring(indexMove + 5, indexFrom - (indexMove + 5)));
                var from = int.Parse(split[i].Substring(indexFrom + 5, indexTo - (indexFrom + 5)));
                var to = int.Parse(split[i].Substring(indexTo + 3, split[i].Length - (indexTo + 3)));

                // ** for answer part one **
                for (int y = 0; y < move; y++)
                {
                    if (floorConfiguration[from - 1].Count > 0)
                    {
                        var item = floorConfiguration[from - 1].Pop();
                        floorConfiguration[to - 1].Push(item);
                    }
                }
                // ** end **

                // ** for answer part two
                //var moveItems = new List<char>();

                //for (int y = 0; y < move; y++)
                //{
                //    if (floorConfiguration[from - 1].Count > 0)
                //    {
                //        moveItems.Add(floorConfiguration[from - 1].Pop());
                //    }
                //}

                //for (int x = moveItems.Count; x > 0; x--)
                //{
                //    floorConfiguration[to - 1].Push(moveItems[x - 1]);

                //}
                // * end *
            }

            // finally print top items from the stack
            foreach (var item in floorConfiguration)
            {
                if (item.Count > 0)
                {
                    // answers
                    Console.WriteLine(item.Peek());
                }
                else
                {
                    Console.WriteLine("Empty");
                }
            }
        }
    }
}