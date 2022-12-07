using System;
namespace AdventofCode2022.Solutions
{
    public class Day6 : SolutionBase
    {
        public Day6(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var packetInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/tuning.txt");

            var dataStream = new List<double>();
            foreach (var data in packetInput)
            {
                dataStream.Add((int)data);
            }

            FindStartOfPacketStream(dataStream, 4, "one");
            FindStartOfPacketStream(dataStream, 14, "two");
        }

        private void FindStartOfPacketStream(List<double> dataStream, int characterAmount, string part)
        {
            for (int i = 0; i < dataStream.Count; i++)
            {
                var uniqueCharacters = new List<double>();
                uniqueCharacters.Add(dataStream[i]);
                for (int y = i + 1; y < dataStream.Count; y++)
                {
                    if (uniqueCharacters.Contains(dataStream[y]))
                    {
                        break;
                    }

                    uniqueCharacters.Add(dataStream[y]);
                    if (uniqueCharacters.Count == characterAmount)
                    {
                        LogPuzzleInformation(6, $"Tuning Trouble {part}");
                        LogPuzzleAnswer((y+1).ToString(), "Tuning Trouble");
                        return;
                    }
                }
            }
        }
    }
}