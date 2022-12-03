using System;
using System.Reflection.PortableExecutable;

namespace AdventofCode2022.Solutions
{
    public class Day2 : SolutionBase
    {
        public Day2(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base (fReader, strUtils, calcUtils)
        {
            var strategySheet = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/rockpaperscissors.txt");
            var plays = stringUtils.SplitStringsOnNewLines(strategySheet);
          
            var answerPartOne = CalculateTotalScore(plays);
            LogPuzzleInformation(2, "Rock Paper Scissors part one");
            LogPuzzleAnswer(answerPartOne, "Rock Paper Scissors");

            var convertedPlays = ConvertPlaysAccordingToStrategy(plays);
            var answerPartTwo = CalculateTotalScore(convertedPlays);
            LogPuzzleInformation(2, "Rock Paper Scissors part two");
            LogPuzzleAnswer(answerPartTwo, "Rock Paper Scissors");
        }

        private int CalculateTotalScore(List<string> plays)
        {
            var score = 0;
            foreach (var play in plays)
            {
                var myWeapon = Enum.TryParse(play[2].ToString(), out ItemValue myWeaponValue);
                score += (int)myWeaponValue;
                score += CalculateWinForWeaponB(play[0], play[2]);
            }
            return score;
        }

        private List<string> ConvertPlaysAccordingToStrategy(List<string> plays)
        {
            var weaponChoices = new List<char>();
            var result = new List<string>();
            foreach (var play in plays)
            {
                var weaponChoice = 'x';
                switch (play[2])
                {
                    case 'X':
                        weaponChoice = CalculateLossWeapon(play[0], play[2]);
                        break;
                    case 'Y':
                        weaponChoice = CalculateDrawWeapon(play[0], play[2]);
                        break;
                    case 'Z':
                        weaponChoice = CalculateWinWeapon(play[0], play[2]);
                        break;
                    default:
                        break;
                }
                weaponChoices.Add(weaponChoice);
            }

            for (int i = 0; i < plays.Count; i++)
            {
                // lekker de spatie er tussen laten want ik haal die er eerder ook niet uit hehe
                result.Add(plays[i][0] + " " + weaponChoices[i]);
            }

            return result;
        }

        private char CalculateLossWeapon(char weaponA, char weaponB)
        {
            switch (weaponA)
            {
                case 'A':
                    return 'Z';
                case 'B':
                    return 'X';
                case 'C':
                    return 'Y';
                default:
                    break;
            }
            return 'Z';
        }

        private char CalculateWinWeapon(char weaponA, char weaponB)
        {
            switch (weaponA)
            {
                case 'A':
                    return 'Y';
                case 'B':
                    return 'Z';
                case 'C':
                    return 'X';
                default:
                    break;
            }
            return 'Z';
        }

        private char CalculateDrawWeapon(char weaponA, char weaponB)
        {
            switch (weaponA)
            {
                case 'A':
                    return 'X';
                case 'B':
                    return 'Y';
                case 'C':
                    return 'Z';
                default:
                    break;
            }
            return 'Z';
        }      

        private int CalculateWinForWeaponB(char weaponA, char weaponB)
        {
            // win
            if (weaponA == 'A' && weaponB == 'Y' || weaponA == 'B' && weaponB == 'Z' || weaponA == 'C' && weaponB == 'X')
            {
                return 6;
            }

            // draw
            if (weaponA == 'A' && weaponB == 'X' || weaponA == 'B' && weaponB == 'Y' || weaponA == 'C' && weaponB == 'Z')
            {
                return 3;
            }

            //loss
            return 0;
        }
    }   

    enum ItemValue
    {
        X = 1,
        Y = 2,
        Z = 3
    }
}