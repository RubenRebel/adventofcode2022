using System;
using System.Numerics;

namespace AdventofCode2022.Solutions
{
    public class Day11 : SolutionBase
    {
        private List<Monkey> Monkeys { get; set; }
        private long Modulo { get; set; }
        public Day11(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var monkeyInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/monkeys.txt");
            var monkeys = stringUtils.GroupStringsOnNewLinesSplit2(monkeyInput);
            PrepareMonkeyData(monkeys);
            Play(20, 3, "one");
            PrepareMonkeyData(monkeys);
            Play(10000, 1, "two");
        }

        private void Play(int rounds, int calm, string part)
        {
            for (int round = 0; round < rounds; round++)
            {
                foreach (var monkey in Monkeys)
                {
                    var transferItems = new List<long>();
                    var removeItems = new List<long>();
                    var transferMonkeys = new List<int>();
                    foreach (var item in monkey.Items)
                    {
                        var operations = monkey.Operation.Split(' ');
                        long increaseWorry = 0;
                        if (operations[1] == "old")
                        {   
                            increaseWorry = item;
                        } else
                        {
                            increaseWorry = long.Parse(operations[1]);
                        }

                        long worryLevel = Calculate(operations[0], item, increaseWorry);
                        long decreasedWorry = worryLevel / calm;
                    
                        decreasedWorry %= Modulo;                   

                        if (decreasedWorry % (long)monkey.TestCondition == 0)
                        {
                            transferMonkeys.Add(monkey.TestSucces);
                        }
                        else
                        {
                            transferMonkeys.Add(monkey.TestFailed);
                        }
                        transferItems.Add(decreasedWorry);
                        removeItems.Add(item);
                        monkey.InspectCounter++;
                    }
                    for (int i = 0; i < transferItems.Count; i++)
                    {
                        Monkeys[(int)transferMonkeys[i]].Items.Add(transferItems[i]);
                        monkey.Items.Remove(removeItems[i]);
                    }
                }
            }
            var activeMonkeys = Monkeys.OrderByDescending(m => m.InspectCounter).Take(2).ToList();
            ulong monkeyBusiness = (ulong)activeMonkeys[0].InspectCounter * (ulong)activeMonkeys[1].InspectCounter;
            LogPuzzleInformation(11, $"Monkey in the Middle part {part}");
            LogPuzzleAnswer(monkeyBusiness.ToString(), $"Monkey in the Middle part {part}");
        }

        private long Calculate(string operatorer, long baseWorry, long increaseWorry)
        {
            switch (operatorer)
            {
                case "+": return baseWorry + increaseWorry;
                case "*": return baseWorry * increaseWorry;
                default: throw new Exception("invalid logic");
            }
        }

        private void PrepareMonkeyData(List<List<string>> monkeyData)
        {
            Monkeys = new List<Monkey>();
            Modulo = 1;
            for (int i = 0; i < monkeyData.Count; i++)
            {
                var monkey = new Monkey();
                monkey.Items = new List<long>();
                monkey.Id = i;

                var items = monkeyData[i][1].Split("Starting items: ");
                var itemIds = items[1].Split(", ");
                foreach (var item in itemIds)
                {
                    var itemId = int.Parse(item);
                    monkey.Items.Add((long)itemId);
                }

                var operation = monkeyData[i][2].Split("old ");
                monkey.Operation = operation[1];

                var test = monkeyData[i][3].Split("divisible by ");
                monkey.TestCondition = int.Parse(test[1]);

                var succeed = monkeyData[i][4].Split("throw to monkey ");
                monkey.TestSucces = int.Parse(succeed[1]);

                var failed = monkeyData[i][5].Split("throw to monkey ");
                monkey.TestFailed = int.Parse(failed[1]);

                Monkeys.Add(monkey);
                Modulo *= monkey.TestCondition;
            }
        }
    }

    internal class Monkey {
        public int Id { get; set; }
        public List<long> Items { get; set; }
        public string Operation { get; set; }
        public int TestCondition { get; set; }
        public int TestSucces { get; set; }
        public int TestFailed { get; set; }
        public int InspectCounter { get; set; } = 0;
    }
}