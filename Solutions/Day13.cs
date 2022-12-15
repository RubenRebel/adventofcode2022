using System;
using System.Xml.Linq;

namespace AdventofCode2022.Solutions
{
    public class Day13 : SolutionBase
    {
        bool Found = false;
        bool SortedPair = false;

        public Day13(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var distressInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/distress.txt");
            var packetPairs = stringUtils.GroupStringsOnNewLinesSplit2(distressInput);

            PartOne(packetPairs);
            PartTwo(packetPairs);
        }

        private void PartTwo(List<List<string>> packetPairs)
        {
            var deviders = new List<string>();
            deviders.Add("[[2]]");
            deviders.Add("[[6]]");
            packetPairs.Add(deviders);

            var packets = new List<Element>();
            var ordered = false;

            for (int pair = 0; pair < packetPairs.Count(); pair++)
            {
                packets.Add(GetPackets(packetPairs[pair][0]));
                packets.Add(GetPackets(packetPairs[pair][1]));
            }

            int stupid = 0;
            while (!ordered && stupid < 10000)
            {
                ordered = true;
                for (int packet = 0; packet < packets.Count() - 1; packet++)
                {
                    Found = false;
                    SortedPair = false;
           
                    var pairOne = packets[packet];
                    var pairTwo = packets[packet + 1];
                   
                    PairSorted(pairOne, pairTwo);
                    if (!SortedPair)
                    {
                        packets[packet] = pairTwo;
                        packets[packet + 1] = pairOne;
                        ordered = false;
                    }
                }
                stupid++;
            }
            var deviderPositions = new List<int>();

            for (int i = 0; i < packets.Count(); i++)
            {
                if (packets[i].Devider == true)
                {
                    deviderPositions.Add(i + 1);
                }
            }
            var decoderKey = 1;
            foreach (var devider in deviderPositions)
            {
                decoderKey *= devider;
            }
            LogPuzzleInformation(12, $"Distress Signal part two");
            LogPuzzleAnswer(decoderKey.ToString(), $"Distress Signal part two");
        }

        private void PartOne(List<List<string>> packetPairs)
        {
            int OrderedPairs = 0;
            for (int pair = 0; pair < packetPairs.Count(); pair++)
            {
                var firstPair = GetPackets(packetPairs[pair][0]);
                var secondPair = GetPackets(packetPairs[pair][1]);
                Found = false;
                SortedPair = false;

                PairSorted(firstPair, secondPair);

                if (SortedPair) {
                    OrderedPairs += pair + 1;
                }
            }
            LogPuzzleInformation(12, $"Distress Signal part one");
            LogPuzzleAnswer(OrderedPairs.ToString(), $"Distress Signal part one");
        }

        private void PairSorted(Element firstPair, Element secondPair)
        {
            if (Found)
            {
                return;
            }

            if (firstPair.Children.Count == 0)
            {
                if(secondPair.Children.Count > 0)
                {
                    firstPair.Children.Add(new Element() { Value = firstPair.Value, Parent = firstPair });
                    PairSorted(firstPair, secondPair);
                    return;
                }

                if (firstPair.Value < secondPair.Value)
                {
                    SortedPair = true;
                    Found = true;
                    return;
                }

                if (firstPair.Value > secondPair.Value)
                {
                    Found = true;
                    return;
                }
            }

            if(secondPair.Children.Count == 0)
            {
                if (firstPair.Children.Count > 0)
                {
                    secondPair.Children.Add(new Element() { Value = secondPair.Value, Parent = secondPair });
                }
            }

            for (int i = 0; i < firstPair.Children.Count(); i++)
            {

                if(secondPair.Children.Count() <= i)
                {
                    Found = true;
                    return;
                }
                PairSorted(firstPair.Children[i], secondPair.Children[i]);
                if (Found)
                {
                    return;
                }

                if (i + 1 == firstPair.Children.Count())
                {
                    if (secondPair.Children.Count() > firstPair.Children.Count())
                    {
                        SortedPair = true;
                        Found = true;
                    }
                }
            }
        }

        private Element GetPackets(string input)
        {
            var result = new Element();
            if (input == "[[2]]" || input == "[[6]]")
            {
                result.Devider = true;
            }

            var currentElement = result;

            var groupItem = "";
            for (int i = 0; i < input.Length; i++)
            {
                var current = input[i];

                if (current == '[')
                {
                    var newEl = new Element();
                    newEl.Parent = currentElement;
                    currentElement.Children.Add(newEl);
                    currentElement = newEl;
                    continue;
                }

                if(current == ',' || current == ']')
                {
                    if (!string.IsNullOrEmpty(groupItem))
                    {
                        var newElement = new Element();
                        newElement.Parent = currentElement;
                        newElement.Value = int.Parse(groupItem);
                        currentElement.Children.Add(newElement);
                        groupItem = "";
                    }

                    if (current == ']')
                    {
                        currentElement = currentElement.Parent;
                    }
                    continue;
                }             

                groupItem += current;
            }
            return result;
        }
    }

    internal class Element
    {
        public Element Parent { get; set; }
        public List<Element> Children { get; set; }
        public int Value { get; set; }
        public bool Devider { get; set; }

        public Element()
        {
            Children = new List<Element>();
        }
    }
}