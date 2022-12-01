using System;
namespace AdventofCode2022
{
    public class CalculationUtilities
    {
        public CalculationUtilities()
        {  }

        public List<int> SumGroupTotals(List<List<int>> input)
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
            return groupTotals;
        }
    }
}

