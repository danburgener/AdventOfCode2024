namespace AdventOfCode2024
{
    public class Day09 : IDay
    {
        private readonly string _fileDayName = "Nine";
        public string GetName() => "Day 09";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            var diskMap = data[0];
            List<int?> fileData = new List<int?>();
            int? currentId = 0;
            for (var i = 0; i < diskMap.Length; i++)
            {
                if (i % 2 == 0)
                {
                    fileData.AddRange(Enumerable.Repeat(currentId, int.Parse(diskMap[i].ToString())));
                }
                else
                {
                    currentId++;
                    fileData.AddRange(Enumerable.Repeat<int?>(null, int.Parse(diskMap[i].ToString())));
                }
            }
            int? lastIndexOfNumber = GetLastIndexOfANumber(fileData, fileData.Count - 1, 0) ?? throw new Exception();
            for (var i = 0; i < lastIndexOfNumber.Value; i++)
            {
                if (fileData[i] is not null)
                {
                    continue;
                }
                else
                {
                    fileData.Swap(i, lastIndexOfNumber.Value);
                    lastIndexOfNumber = GetLastIndexOfANumber(fileData, lastIndexOfNumber.Value, i);
                }
            }
            var lastIndexNotNull = fileData.FindLastIndex(i => i is not null);
            long count = 0;
            for (var i = 0; i <= lastIndexNotNull; i++)
            {
                count += (fileData[i].Value * i);
            }
            return count;
        }

        private int? GetLastIndexOfANumber(List<int?> fileData, int lastKnownIndexOfNumber, int minIndex)
        {
            for (var i = lastKnownIndexOfNumber; i > minIndex; i--)
            {
                if (fileData[i] is not null)
                {
                    return i;
                }
            }
            return null;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }
    }
}
