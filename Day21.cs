﻿namespace AdventOfCode2024
{
    public class Day21 : IDay
    {
        private readonly string _fileDayName = "TwentyOne";
        public string GetName() => "Day 21";

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