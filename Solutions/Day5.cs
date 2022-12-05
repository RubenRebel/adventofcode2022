using System;
namespace AdventofCode2022.Solutions
{
    public class Day5 : SolutionBase
    {
        private int _startOfMovesIndex = 0;
        private List<Stack<char>> _floorConfiguration;
        private int _floorConfigLength = 0;
        private List<string> _split;

        public Day5(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var rearrangeSheet = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/crates.txt");
            _split = stringUtils.SplitStringsOnNewLines(rearrangeSheet);
            _floorConfigLength = (_split[0].Length + 1) / 4;

            LoadFloorConfiguration();
            GetSmallCraneAnswer();

            LoadFloorConfiguration();
            GetBigCraneAnswer();     
        }

        private void GetSmallCraneAnswer()
        {
            for (int i = _startOfMovesIndex; i < _split.Count; i++)
            {
                var indexMove = _split[i].IndexOf("move ");
                var indexFrom = _split[i].IndexOf("from ");
                var indexTo = _split[i].IndexOf("to ");

                var move = int.Parse(_split[i].Substring(indexMove + 5, indexFrom - (indexMove + 5)));
                var from = int.Parse(_split[i].Substring(indexFrom + 5, indexTo - (indexFrom + 5)));
                var to = int.Parse(_split[i].Substring(indexTo + 3, _split[i].Length - (indexTo + 3)));

                for (int y = 0; y < move; y++)
                {
                    if (_floorConfiguration[from - 1].Count > 0)
                    {
                        var item = _floorConfiguration[from - 1].Pop();
                        _floorConfiguration[to - 1].Push(item);
                    }
                }         
            }
            PrintAnswer("one");
        }

        private void GetBigCraneAnswer()
        {
            for (int i = _startOfMovesIndex; i < _split.Count; i++)
            {
                var indexMove = _split[i].IndexOf("move ");
                var indexFrom = _split[i].IndexOf("from ");
                var indexTo = _split[i].IndexOf("to ");

                var move = int.Parse(_split[i].Substring(indexMove + 5, indexFrom - (indexMove + 5)));
                var from = int.Parse(_split[i].Substring(indexFrom + 5, indexTo - (indexFrom + 5)));
                var to = int.Parse(_split[i].Substring(indexTo + 3, _split[i].Length - (indexTo + 3)));

                var moveItems = new List<char>();

                for (int y = 0; y < move; y++)
                {
                    if (_floorConfiguration[from - 1].Count > 0)
                    {
                        moveItems.Add(_floorConfiguration[from - 1].Pop());
                    }
                }

                for (int x = moveItems.Count; x > 0; x--)
                {
                    _floorConfiguration[to - 1].Push(moveItems[x - 1]);

                }
            }
            PrintAnswer("two");
        }

        private void PrintAnswer(string part)
        {
            var result = "";
            foreach (var item in _floorConfiguration)
            {
                if (item.Count > 0)
                {           
                    result += item.Peek();
                }
            }
            LogPuzzleInformation(5, $"Supply Stacks part {part}");
            LogPuzzleAnswer(result, "Camp Cleanup");
        }

        private List<Stack<char>> LoadFloorConfiguration()
        {
            var result = new List<Stack<char>>();
            var cratesConfiguration = new List<string>();
            _floorConfiguration = new List<Stack<char>>();

            for (int i = 0; i < _floorConfigLength; i++)
            {
                _floorConfiguration.Add(new Stack<char>());
            }

            for (int i = 0; i < _split.Count; i++)
            {
                if (string.IsNullOrEmpty(_split[i]))
                {
                    _startOfMovesIndex = i + 1;
                    break;
                }
                cratesConfiguration.Add(_split[i]);
            }

            var spot = cratesConfiguration.LastOrDefault();

            for (int i = cratesConfiguration.Count - 2; i >= 0; i--)
            {
                for (int y = 0; y < cratesConfiguration[i].Length; y++)
                {
                    if (cratesConfiguration[i][y] != ' ' && cratesConfiguration[i][y] != '[' && cratesConfiguration[i][y] != ']')
                    {
                        var crateIndex = int.Parse(spot[y].ToString());
                        _floorConfiguration[crateIndex - 1].Push(cratesConfiguration[i][y]);
                    }
                }
            }
            return result;
        }
    }
}