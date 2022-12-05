using System;
namespace AdventofCode2022.Solutions
{
    public class SolutionBase
    {
        protected FileReader fileReader;
        protected StringUtilities stringUtils;
        protected CalculationUtilities calculationUtils;

        public SolutionBase(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils)
        {
            fileReader = fReader;
            stringUtils = strUtils;
            calculationUtils = calcUtils;
        }

        public void LogPuzzleInformation(int day, string puzzleName)
        {
            Console.WriteLine($"--- Day {day}: {puzzleName} ---");
        }

        public void LogPuzzleAnswer(string answer, string puzzleName)
        {
            Console.WriteLine($"--- Answer for puzzle {puzzleName}: {answer} ---");
            Console.WriteLine();
        }
    }
}