using System;
namespace AdventofCode2022.Solutions
{
    public class Day3 : SolutionBase
    {
        public Day3(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var sackSheet = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/sack.txt");
            var sackContents = stringUtils.SplitStringsOnNewLines(sackSheet);
            var answerPartOne = 0;
            foreach (var content in sackContents)
            {
                var compartmentOne = content.Substring(0, content.Length / 2);
                var compartmentTwo = content.Substring(content.Length / 2);
                foreach (var item in compartmentOne)
                {
                    if (compartmentTwo.Contains(item))
                    {
                        answerPartOne += PriorityOfItem(item);
                        break;
                    }                   
                }
            }
            LogPuzzleInformation(3, "Rucksack Reorganization part one");
            LogPuzzleAnswer(answerPartOne, "Rucksack Reorganization");

            var index = 0;
            var answerPartTwo = 0;
            while (index + 2 < sackContents.Count)
            {
                foreach (var item in sackContents[index])
                {
                    if (sackContents[index+1].Contains(item) && sackContents[index + 2].Contains(item))
                    {
                        answerPartTwo += PriorityOfItem(item);
                        index += 3;
                        break;
                    }
                }             
            }
            LogPuzzleInformation(3, "Rucksack Reorganization part two");
            LogPuzzleAnswer(answerPartTwo, "Rucksack Reorganization");
        }

        private int PriorityOfItem(char item)
        {
            var alphaBetIndex = (int)item % 32;
            if (char.IsUpper(item)){
                return alphaBetIndex + 26;
            }
            return alphaBetIndex;
        }
    }
}