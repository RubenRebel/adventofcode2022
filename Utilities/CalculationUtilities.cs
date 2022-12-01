using System;
namespace AdventofCode2022
{
    public class CalculationUtilities
    {
        public CalculationUtilities()
        {  }

        public int FindLargestGroup(List<List<int>> input, int amountOfGroups)
        {
            var groupTotals = new List<int>();
            foreach (var group in input)
            {
                var total = 0;
                foreach (var item in group)
                {
                    total += item;
                }
                groupTotals.Add(total);
            }
            return groupTotals.OrderByDescending(g => g).Select(g => g).Take(amountOfGroups).Sum();
        }
    }
}

