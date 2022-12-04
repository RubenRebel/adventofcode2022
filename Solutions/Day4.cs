using System;

namespace AdventofCode2022.Solutions
{
    public class Day4 : SolutionBase
    {
        public Day4(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var sectionsSheet = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/sections.txt");
            var sectionPairs = stringUtils.SplitStringsOnNewLines(sectionsSheet);

            var answerPartOne = 0;
            var answerPartTwo = 0;
            foreach (var section in sectionPairs)
            {
                var sections = section.Split(',');
                var rangeOne = sections[0].Split('-');
                var rangeTwo = sections[1].Split('-');

                if (int.Parse(rangeOne[0]) >= int.Parse(rangeTwo[0]) && int.Parse(rangeOne[0]) <= int.Parse(rangeTwo[1])
                    && int.Parse(rangeOne[1]) >= int.Parse(rangeTwo[0]) && int.Parse(rangeOne[1]) <= int.Parse(rangeTwo[1]) ||
                    int.Parse(rangeTwo[0]) >= int.Parse(rangeOne[0]) && int.Parse(rangeTwo[0]) <= int.Parse(rangeOne[1])
                    && int.Parse(rangeTwo[1]) >= int.Parse(rangeOne[0]) && int.Parse(rangeTwo[1]) <= int.Parse(rangeOne[1]))
                {
                    answerPartOne += 1;
                }

                if (int.Parse(rangeOne[0]) >= int.Parse(rangeTwo[0]) && int.Parse(rangeOne[0]) <= int.Parse(rangeTwo[1])
                    || int.Parse(rangeOne[1]) >= int.Parse(rangeTwo[0]) && int.Parse(rangeOne[1]) <= int.Parse(rangeTwo[1]) ||
                    int.Parse(rangeTwo[0]) >= int.Parse(rangeOne[0]) && int.Parse(rangeTwo[0]) <= int.Parse(rangeOne[1])
                    || int.Parse(rangeTwo[1]) >= int.Parse(rangeOne[0]) && int.Parse(rangeTwo[1]) <= int.Parse(rangeOne[1]))
                {
                    answerPartTwo += 1;
                }
            }
        }
    }
}