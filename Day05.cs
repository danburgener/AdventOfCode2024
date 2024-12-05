

namespace AdventOfCode2024
{
    public static class Day05
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Five", "One");
            Dictionary<int, List<int>> pageOrderingRules;
            List<List<int>> pagesToProduce;
            ParseData(data, out pageOrderingRules, out pagesToProduce);
            int count = 0;
            foreach (var pageToProduce in pagesToProduce)
            {
                if (InCorrectOrder(pageToProduce, pageOrderingRules))
                {
                    count += pageToProduce[pageToProduce.Count / 2];
                }
            }
            return count;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Five", "Two");
            Dictionary<int, List<int>> pageOrderingRules;
            List<List<int>> pagesToProduce;
            ParseData(data, out pageOrderingRules, out pagesToProduce);
            int count = 0;
            foreach (var pageToProduce in pagesToProduce)
            {
                if (!InCorrectOrder(pageToProduce, pageOrderingRules))
                {
                    List<int> fixedOrder = FixedOrder(pageToProduce.ToList(), pageOrderingRules);
                    count += fixedOrder[fixedOrder.Count / 2];
                }
            }
            return count;
        }

        //private static List<int> FixedOrder(List<int> pageToProduce, Dictionary<int, List<int>> pageOrderingRules)
        //{
        //    bool inCorrectOrder = false;
        //    pageToProduce.Sort();
        //    List<int> distinctNumbers = pageToProduce.Distinct().ToList();
        //    while (!inCorrectOrder)
        //    {
        //        foreach (var distinctNumber in distinctNumbers)
        //        {
        //            if (pageOrderingRules.ContainsKey(distinctNumber))
        //            {
        //                var rules = pageOrderingRules[distinctNumber];
        //                foreach (var rule in rules)
        //                {
        //                    int lastIndexOfKey = pageToProduce.LastIndexOf(distinctNumber);
        //                    int removedCount = pageToProduce.RemoveAll(p => p == rule);
        //                    pageToProduce.InsertRange(lastIndexOfKey, Enumerable.Repeat(rule, removedCount));
        //                }
        //            }
        //        }
        //        if (InCorrectOrder(pageToProduce, pageOrderingRules))
        //        {
        //            return pageToProduce;
        //        }
        //    }
        //    return pageToProduce;
        //}

        private static List<int> FixedOrder(List<int> pageToProduce, Dictionary<int, List<int>> pageOrderingRules)
        {
            bool inCorrectOrder = false;
            int attempts = 0;
            while (!inCorrectOrder && attempts < 50000)
            {
                attempts++;
                for (var i = 0; i < pageToProduce.Count; i++)
                {
                    var page = pageToProduce[i];
                    bool somethingChanged = false;
                    if (pageOrderingRules.ContainsKey(page))
                    {
                        var rules = pageOrderingRules[page];
                        for (var j = 0; j < i; j++)
                        {
                            foreach (var rule in rules)
                            {
                                if (pageToProduce[j] == rule)
                                {
                                    pageToProduce.RemoveAt(j);
                                    pageToProduce.Insert(i, rule);
                                    somethingChanged = true;
                                    break;
                                }
                            }
                            if (somethingChanged)
                            {
                                break;
                            }
                        }
                    }
                    if (somethingChanged)
                    {
                        break;
                    }
                }
                inCorrectOrder = InCorrectOrder(pageToProduce, pageOrderingRules);
            }
            return inCorrectOrder ? pageToProduce : throw new Exception();
        }

        private static void ParseData(string[] data, out Dictionary<int, List<int>> pageOrderingRules, out List<List<int>> pagesToProduce)
        {
            pageOrderingRules = new Dictionary<int, List<int>>();
            pagesToProduce = new List<List<int>>();
            bool foundSpace = false;
            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    foundSpace = true;
                    continue;
                }
                if (!foundSpace)
                {
                    var splitLine = line.Split('|');
                    int key = int.Parse(splitLine[0]);
                    int value = int.Parse(splitLine[1]);
                    if (pageOrderingRules.ContainsKey(key))
                    {
                        pageOrderingRules[key].Add(value);
                    }
                    else
                    {
                        pageOrderingRules.Add(key, new List<int>() { value });
                    }
                }
                else
                {
                    var splitLine = line.Split(',').Select(s => int.Parse(s)).ToList();
                    pagesToProduce.Add(splitLine);
                }
            }
        }

        private static bool InCorrectOrder(List<int> pageToProduce, Dictionary<int, List<int>> pageOrderingRules)
        {
            for (var i = 0; i < pageToProduce.Count(); i++)
            {
                var page = pageToProduce[i];
                if (pageOrderingRules.ContainsKey(page))
                {
                    var rules = pageOrderingRules[page];
                    for (var j = 0; j < i; j++)
                    {
                        foreach (var rule in rules)
                        {
                            if (pageToProduce[j] == rule)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            return true;
        }

        static IList<IList<int>> Permute(int[] nums)
        {
            var list = new List<IList<int>>();
            return DoPermute(nums, 0, nums.Length - 1, list);
        }

        static IList<IList<int>> DoPermute(int[] nums, int start, int end, IList<IList<int>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<int>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref nums[start], ref nums[i]);
                    DoPermute(nums, start + 1, end, list);
                    Swap(ref nums[start], ref nums[i]);
                }
            }

            return list;
        }

        static void Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

    }
}
