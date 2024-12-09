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

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            var diskMap = data[0];
            List<int?> fileData = new List<int?>();
            List<int> fileIds = new List<int>();
            int? currentId = 0;
            for (var i = 0; i < diskMap.Length; i++)
            {
                int currentNumber = int.Parse(diskMap[i].ToString());
                if (i % 2 == 0)
                {
                    fileIds.Add(currentId.Value);
                    fileData.AddRange(Enumerable.Repeat(currentId, currentNumber));
                }
                else
                {
                    currentId++;
                    fileData.AddRange(Enumerable.Repeat<int?>(null, currentNumber));
                }
            }
            int lowestEmptyBlockIndex = 0;
            for (var i = fileIds.Count - 1; i >= 0; i--)
            {
                var fileId = fileIds[i];
                (int fileIdBlockIndex, int fileIdBlockLength) = FindNumberBlock(fileData, fileId);
                (int nextEmptyBlockIndex, int nextEmptyBlockLength) = NextEmptyBlock(lowestEmptyBlockIndex, fileData);
                lowestEmptyBlockIndex = nextEmptyBlockIndex;
                while (nextEmptyBlockIndex != -1 && nextEmptyBlockIndex < fileIdBlockIndex)
                {
                    if (nextEmptyBlockLength >= fileIdBlockLength)
                    {
                        for (var j = 0; j < fileIdBlockLength; j++)
                        {
                            fileData.Swap(fileIdBlockIndex + j, nextEmptyBlockIndex + j);
                        }
                        break;
                    }
                    (nextEmptyBlockIndex, nextEmptyBlockLength) = NextEmptyBlock(nextEmptyBlockIndex + nextEmptyBlockLength, fileData);
                }
            }

            long count = 0;
            for (var i = 0; i < fileData.Count(); i++)
            {
                if (fileData[i].HasValue)
                {
                    count += (fileData[i].Value * i);
                }
            }
            return count;
        }

        private (int index, int length) FindNumberBlock(List<int?> fileData, int number)
        {
            var index = fileData.FindIndex(i => i == number);
            var endIndex = fileData.FindLastIndex(i => i == number);
            return (index, endIndex - index + 1);
        }

        private (int index, int length) NextEmptyBlock(int startingIndex, List<int?> fileData)
        {
            var index = fileData.FindIndex(startingIndex, i => i is null);
            var endIndex = fileData.FindIndex(index, i => i is not null);
            return (index, endIndex - index);
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
    }
}
