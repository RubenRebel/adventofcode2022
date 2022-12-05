using System;
namespace AdventofCode2022.Solutions
{
    public class Day1 : SolutionBase
    {
        public Day1(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var calories = PrepareCalorieData();
            var orderedGroupTotals = calculationUtils.SumGroupTotals(calories).OrderByDescending(g => g);

            var answerPartOne = orderedGroupTotals.Take(1).Sum();
            LogPuzzleInformation(1, "Calorie Counting part one");
            LogPuzzleAnswer(answerPartOne.ToString(), "Calorie Counting");

            var answerPartTwo = orderedGroupTotals.Take(3).Sum();
            LogPuzzleInformation(1, "Calorie Counting part two");
            LogPuzzleAnswer(answerPartTwo.ToString(), "Calorie Counting");
        }

        private List<List<int>> PrepareCalorieData()
        {
            var elfCalorieContent = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/ElvesCalories.txt");
            var caloriesGroupedToString = stringUtils.GroupStringsOnNewLinesSplit(elfCalorieContent);
            var caloriesGroupedToInt = new List<List<int>>();

            foreach (var stringList in caloriesGroupedToString)
            {
                caloriesGroupedToInt.Add(stringUtils.ConvertStringListToIntList(stringList));
            }
            return caloriesGroupedToInt;
        }
    }
}