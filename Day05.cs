namespace AdventOfCode2024
{
    public class Day05 : IDay
    {
        public string GetName() => "Day 05";

        public async Task<long> One()
        {
            var data = await Common.ReadFile("Five", "One");
            ParseData(data, out Dictionary<int, List<int>> pageOrderingRules, out List<List<int>> pagesToProduce);
            int count = 0;
            foreach (var pageToProduce in pagesToProduce)
            {
                if (IsOrderCorrect(pageToProduce, pageOrderingRules))
                {
                    count += pageToProduce[pageToProduce.Count / 2];
                }
            }
            return count;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile("Five", "Two");
            ParseData(data, out Dictionary<int, List<int>> pageOrderingRules, out List<List<int>> pagesToProduce);
            int count = 0;
            foreach (var pageToProduce in pagesToProduce)
            {
                if (!IsOrderCorrect(pageToProduce, pageOrderingRules))
                {
                    List<int> fixedOrder = FixedOrder(pageToProduce.ToList(), pageOrderingRules);
                    count += fixedOrder[fixedOrder.Count / 2];
                }
            }
            return count;
        }

        private static List<int> FixedOrder(List<int> pageToProduce, Dictionary<int, List<int>> pageOrderingRules)
        {
            bool inCorrectOrder = false;
            int attempts = 0;
            while (!inCorrectOrder && attempts < 1000)
            {
                attempts++;
                for (var padeToProduceIndex = 0; padeToProduceIndex < pageToProduce.Count; padeToProduceIndex++)
                {
                    var page = pageToProduce[padeToProduceIndex];
                    bool somethingChanged = false;
                    if (pageOrderingRules.TryGetValue(page, out List<int>? rules))
                    {
                        for (var j = 0; j < padeToProduceIndex; j++)
                        {
                            foreach (var rule in rules)
                            {
                                if (pageToProduce[j] == rule)
                                {
                                    pageToProduce.RemoveAt(j);
                                    pageToProduce.Add(rule);
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
                inCorrectOrder = IsOrderCorrect(pageToProduce, pageOrderingRules);
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

        private static bool IsOrderCorrect(List<int> pageToProduce, Dictionary<int, List<int>> pageOrderingRules)
        {
            for (var i = 0; i < pageToProduce.Count(); i++)
            {
                var page = pageToProduce[i];
                if (pageOrderingRules.TryGetValue(page, out List<int>? rules))
                {
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
    }
}
