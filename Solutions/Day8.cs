using System;
namespace AdventofCode2022.Solutions
{
    public class Day8 : SolutionBase
    {
        List<List<int>> Grid = new List<List<int>>();
        int AmountOfTreesVisible = 0;
        int HighestScenicScore = 0;
        public Day8(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var treeHouseInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/treehouse.txt");
            var split = stringUtils.SplitStringsOnNewLines(treeHouseInput);
            var edgeSize = 1;

            foreach (var line in split)
            {
                var row = new List<int>();
                foreach (var number in line)
                {
                    row.Add(int.Parse(number.ToString()));
                }
                Grid.Add(row);
            }

            for (int row = edgeSize; row < Grid.Count - edgeSize; row++)
            {
                for (int column = edgeSize; column < Grid[row].Count - edgeSize; column++)
                {
                    var tree = Grid[row][column];
                    // tree visible logic
                    if(TreeVisible(row, column, tree))
                    {
                        AmountOfTreesVisible++;
                    }
                }
            }
            var surroundingTreesAmount = (Grid[0].Count * 2) + (Grid.Count * 2) - 4;
            AmountOfTreesVisible += surroundingTreesAmount;

            LogPuzzleInformation(8, "Treetop Tree House part one");
            LogPuzzleAnswer(AmountOfTreesVisible.ToString(), "Treetop Tree House part one");
            LogPuzzleInformation(8, "Treetop Tree House part two");
            LogPuzzleAnswer(HighestScenicScore.ToString(), "Treetop Tree House part two");
        }

        private bool TreeVisible(int startRow, int startColumn, int treeSize)
        {
            var inVisible = 0;
            var top = 0;
            var bottom = 0;
            var left = 0;
            var right = 0;
            // check right
            for (int i = startColumn + 1; i < Grid[startRow].Count; i++)
            {
                right++;
                if (Grid[startRow][i] >= treeSize)
                {
                    inVisible++;
                    break;
                }
            }

            //check top
            for (int i = startRow - 1; i >= 0; i--)
            {
                top++;
                if (Grid[i][startColumn] >= treeSize)
                {
                    inVisible++;
                    break;
                }
            }

            //check bottom
            for (int i = startRow + 1; i < Grid.Count; i++)
            {
                bottom++;
                if (Grid[i][startColumn] >= treeSize)
                {
                    inVisible++;
                    break;
                }
            }

            // check left
            for (int i = startColumn - 1; i >= 0; i--)
            {
                left++;
                if (Grid[startRow][i] >= treeSize)
                {
                    inVisible++;
                    break;
                }
            }

            var scenicScore = top * left * bottom * right;
            if(scenicScore > HighestScenicScore)
            {
                HighestScenicScore = scenicScore;
            }

            if(inVisible == 4)
            {
                return false;
            }
            return true;
        }
    }
}

