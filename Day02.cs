
namespace AdventOfCode2024
{
    public static class Day02
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Two", "One");
            int safeReports = 0;

            foreach (var line in data)
            {
                var levels = line.Split(' ').Select(l => int.Parse(l)).ToArray();
                if (ReportIsSafe(levels))
                {
                    safeReports++;
                }
            }
            return safeReports;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Two", "Two");
            int safeReports = 0;

            foreach (var line in data)
            {
                var levels = line.Split(' ').Select(l => int.Parse(l)).ToArray();
                if (ReportIsSafe(levels))
                {
                    safeReports++;
                }
                else
                {
                    for (int i = 0; i < levels.Length; i++)
                    {
                        var newLevel = levels.ToList();
                        newLevel.RemoveAt(i);
                        if (ReportIsSafe(newLevel.ToArray()))
                        {
                            safeReports++;
                            break;
                        }
                    }
                }
            }
            return safeReports;
        }

        private static bool ReportIsSafe(int[] levels)
        {
            var isIncreasing = levels[0] < levels[1];
            for (var i = 0; i < levels.Length - 1; i++)
            {
                int diff = Math.Abs(levels[i] - levels[i + 1]);
                if ((diff >= 1 && diff <= 3) && isIncreasing == levels[i] < levels[i + 1])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
