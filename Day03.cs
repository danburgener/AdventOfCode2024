
using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    public class Day03 : IDay
    {
        private readonly string _fileDayName = "Three";
        public string GetName() => "Day 03";
        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            MatchCollection matches = regex.Matches(string.Join(' ', data));
            int total = 0;
            foreach (Match match in matches)
            {
                total += (int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
            }

            return total;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            string pattern = @"(?:mul\((\d{1,3}),(\d{1,3})\))|(?:do\(\))|(?:don\'t\(\))";
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            MatchCollection matches = regex.Matches(string.Join(' ', data));
            int total = 0;
            bool enabled = true;
            foreach (Match match in matches)
            {
                if (match.Value == "don't()")
                {
                    enabled = false;
                }
                else if (match.Value == "do()")
                {
                    enabled = true;
                }
                else
                {
                    if (enabled)
                    {
                        total += (int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
                    }
                }
            }

            return total;
        }
    }
}
