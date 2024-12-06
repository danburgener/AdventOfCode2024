namespace AdventOfCode2024
{
    public class Day22 : IDay
    {
        private readonly string _fileDayName = "TwentyTwo";
        public string GetName() => "Day 22";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            int count = 0;
            return count;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }
    }
}
