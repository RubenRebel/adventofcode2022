using System;

namespace AdventofCode2022
{
    public class StringUtilities
    {
        public StringUtilities()
        { }

        public List<List<string>> GroupStringsOnNewLinesSplit(string input)
        {
            var split = input.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            var result = new List<List<string>>();
            var stringGroup = new List<string>();

            foreach (var stringItem in split)
            {
                if (string.IsNullOrEmpty(stringItem))
                {
                    result.Add(stringGroup);
                    stringGroup = new List<string>();
                } else
                {
                    stringGroup.Add(stringItem);
                }
            }
            return result;
        }

        public List<List<string>> GroupStringsOnNewLinesSplit2(string input)
        {
            var split = input.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            var result = new List<List<string>>();
            var stringGroup = new List<string>();

            foreach (var stringItem in split)
            {
                if (string.IsNullOrEmpty(stringItem))
                {
                    result.Add(stringGroup);
                    stringGroup = new List<string>();
                }
                else
                {
                    stringGroup.Add(stringItem);
                }
            }
            result.Add(stringGroup);
            return result;
        }

        public List<string> SplitStringsOnNewLines(string input)
        {
            var split = input.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);           
            return split.ToList();
        }

        public List<int> ConvertStringListToIntList(List<string> input)
        {
            var output = new List<int>();
            foreach (var stringItem in input)
            {
                if (Int32.TryParse(stringItem, out int intItem))
                {
                    output.Add(intItem);
                }
                else
                {
                    Console.WriteLine($"Could not parse {stringItem} to int");
                }
            }
            return output;
        }


    }
}