using System;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace AdventofCode2022.Solutions
{
    public class Day10 : SolutionBase
    {
        int RegisterX = 1;
        Queue<int> Tasks = new Queue<int>();
        int SignalStrengthSum = 0;

        public Day10(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var cathodesInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/Cathode.txt");
            LoadTasks(cathodesInput);
            PartOne();
            LoadTasks(cathodesInput);
            PartTwo();
        }

        private void PartTwo()
        {
            RegisterX = 1;
            var cycle = 1;
            var cycleLength = 40;
            int task = Tasks.Dequeue();
            int taskCount = 1;

            LogPuzzleInformation(10, "Cathode-Ray Tube part two");


            while (Tasks.Count() > 0)
            {
                var rowIndex = 0;

                while (rowIndex < cycleLength)
                {
                    if (rowIndex == RegisterX || rowIndex == (RegisterX - 1) || rowIndex == (RegisterX + 1))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                    cycle++;
                    rowIndex++;

                    if(taskCount == 2)
                    {    
                        RegisterX += task;
                        
                        if (Tasks.Count() > 0)
                        {
                            task = Tasks.Dequeue();
                        }
                        if(task != 0)
                        {
                            taskCount = 1;
                        }
                    } else
                    {
                        taskCount++;
                    }                    
                }
                Console.WriteLine();
            }
        }

        private void PartOne()
        {
            var cycle = 1;
            while (Tasks.Count() > 0)
            {
                var task = Tasks.Dequeue();

                if (task != 0)
                {
                    cycle++;
                    if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
                    {
                        SignalStrengthSum += cycle * RegisterX;
                    }
                    RegisterX += task;

                }

                cycle++;

                if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
                {
                    SignalStrengthSum += cycle * RegisterX;
                }
            }
            LogPuzzleInformation(10, "Cathode-Ray Tube part one");
            LogPuzzleAnswer(SignalStrengthSum.ToString(), "Cathode-Ray Tube part one");
        }

        private void LoadTasks(string cathodesInput)
        {
            var cathodes = stringUtils.SplitStringsOnNewLines(cathodesInput);

            for (int i = 0; i < cathodes.Count; i++)
            {
                if (cathodes[i].Contains("addx"))
                {
                    var split = cathodes[i].Split(' ');
                    Tasks.Enqueue(int.Parse(split[1].ToString()));
                }
                else
                {
                    Tasks.Enqueue(0);
                }
            }
        }
    }
}