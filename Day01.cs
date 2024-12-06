
namespace AdventOfCode2024
{
    public class Day01 : IDay
    {
        private readonly string _fileDayName = "One";
        public string GetName() => "Day 01";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            var (leftList, rightList) = GetLists(data);
            leftList.Sort();
            rightList.Sort();
            int currentDistance = 0;
            for (int i = 0; i < leftList.Count; i++)
            {

                currentDistance += Math.Abs(leftList[i] - rightList[i]);
            }
            return currentDistance;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            var (leftList, rightList) = GetLists(data);
            int similarity = 0;
            foreach (var leftValue in leftList)
            {
                var count = rightList.Count(r => r == leftValue);
                similarity += (leftValue * count);

            }
            return similarity;
        }

        private static (List<int> left, List<int> right) GetLists(string[] lines)
        {
            List<int> leftList = new();
            List<int> rightList = new();
            foreach (var line in lines)
            {
                var splitLine = line.Split(' ');
                splitLine = splitLine.Where(s => int.TryParse(s, out _)).ToArray();
                leftList.Add(int.Parse(splitLine[0]));
                rightList.Add(int.Parse(splitLine[1]));
            }

            return (leftList, rightList);
        }
    }
}
