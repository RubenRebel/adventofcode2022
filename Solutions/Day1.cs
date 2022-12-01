using System;
namespace AdventofCode2022
{
    public class Day1
    {
        private FileReader _fileReader;
        private StringUtilities _stringUtils;
        private CalculationUtilities _calculationUtils;

        public Day1(FileReader fileReader, StringUtilities stringUtils, CalculationUtilities calculationUtils)
        {
            _fileReader = fileReader;
            _stringUtils = stringUtils;
            _calculationUtils = calculationUtils;

            var calories = PrepareCalorieData();

            var answerPartOne = GetLargestTotalCaloriesForGroups(calories, 1);
            _stringUtils.LogPuzzleInformation(1, "Calorie Counting part one");
            _stringUtils.LogPuzzleAnswer(answerPartOne, "Calorie Counting");

            var answerPartTwo = GetLargestTotalCaloriesForGroups(calories, 3);
            _stringUtils.LogPuzzleInformation(1, "Calorie Counting part two");
            _stringUtils.LogPuzzleAnswer(answerPartTwo, "Calorie Counting");
        }

        private List<List<int>> PrepareCalorieData()
        {          
            var elfCalorieContent = _fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/ElvesCalories.txt");
            var caloriesGroupedToString = _stringUtils.SplitStringsOnNewLines(elfCalorieContent);
            var caloriesGroupedToInt = new List<List<int>>();

            foreach (var stringList in caloriesGroupedToString)
            {
                caloriesGroupedToInt.Add(_stringUtils.ConvertStringListToIntList(stringList));
            }
            return caloriesGroupedToInt;
        }

        private int GetLargestTotalCaloriesForGroups(List<List<int>> caloriesByGroup, int groups)
        {
            return _calculationUtils.FindLargestGroup(caloriesByGroup, groups);
        }
    }
}

