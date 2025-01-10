
namespace AdventOfCode2024
{
    public class Day25 : IDay
    {
        private readonly string _fileDayName = "TwentyFive";
        public string GetName() => "Day 25";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<string> lines = new List<string>();
            List<Key> keys = new();
            List<Lock> locks = new();
            for(var lineIndex = 0; lineIndex < data.Length; lineIndex++)
            {
                if (string.IsNullOrEmpty(data[lineIndex]) || lineIndex == data.Length-1)
                {
                    if (lines[0][0] == '.')
                    {
                        keys.Add(new Key() { Heights = GenerateHeights(lines) });
                    }
                    else
                    {
                        locks.Add(new Lock() { Heights = GenerateHeights(lines) });
                    }
                    lines = new List<string>();
                }
                else
                {
                    lines.Add(data[lineIndex]);
                }
            }
            int validCombos = 0;
            foreach(var key in keys)
            {
                foreach(var lockItem in locks){
                    if (ValidCombos(key, lockItem, 6))
                    {
                        validCombos++;
                    }
                }
            }
            return validCombos;
        }

        private static bool ValidCombos(Key key, Lock lockItem, int maxHeight)
        {
            for(var index = 0; index < key.Heights.Count; index++)
            {
                if (key.Heights[index] + lockItem.Heights[index] >= maxHeight)
                {
                    return false;
                }
            }
            return true;
        }

        private List<int> GenerateHeights(List<string> lines)
        {
            List<int> heights = new();
            for(var column = 0; column < lines[0].Length; column++)
            {
                int columnHeight = 0;
                for(var row = 0; row < lines.Count; row++)
                {
                    if (lines[row][column] == '#')
                    {
                        columnHeight++;
                    }
                }
                heights.Add(columnHeight-1);
            }
            return heights;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        public record Lock
        {
            public required List<int> Heights { get; set; }
        }
        public record Key
        {
            public required List<int> Heights { get; set; }
        }
    }
}
