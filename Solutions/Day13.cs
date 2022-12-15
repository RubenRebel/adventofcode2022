using System;
using System.Xml.Linq;

namespace AdventofCode2022.Solutions
{
    public class Day13 : SolutionBase
    {
        int OrderedPairs = 0;
        bool Found = false;
        public Day13(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var distressInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/distress.txt");
            var packetPairs = stringUtils.GroupStringsOnNewLinesSplit2(distressInput);

            for (int pair = 0; pair < packetPairs.Count(); pair++)
            {
                var firstPair = GetPackets(packetPairs[pair][0]);
                var secondPair = GetPackets(packetPairs[pair][1]);
                Found = false;
                ComparePairs(firstPair, secondPair, pair + 1);
            }
        }

        private void ComparePairs(Element firstPair, Element secondPair, int pairIndex)
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
                    ComparePairs(firstPair, secondPair, pairIndex);
                    return;
                }

                if (firstPair.Value < secondPair.Value)
                {
                    OrderedPairs += pairIndex;
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
                ComparePairs(firstPair.Children[i], secondPair.Children[i], pairIndex);
                if (Found)
                {
                    return;
                }

                if (i + 1 == firstPair.Children.Count())
                {
                    if (secondPair.Children.Count() > firstPair.Children.Count())
                    {
                        OrderedPairs += pairIndex;
                        Found = true;
                    }
                }
            }
        }

        private Element GetPackets(string input)
        {
            var result = new Element();

            var currentElement = result;

            var groupItem = "";
            for (int i = 1; i < input.Length; i++)
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

        public Element()
        {
            Children = new List<Element>();
        }
    }
}