namespace AdventOfCode2024
{
    public class Day19 : IDay
    {
        private readonly string _fileDayName = "Nineteen";
        public string GetName() => "Day 19";

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
