

namespace AdventOfCode2024
{
    public class Day11 : IDay
    {
        private readonly string _fileDayName = "Eleven";
        public string GetName() => "Day 11";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            var initialStones = data[0].Split(' ').ToList();
            return ProcessStones(25, initialStones);
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            var initialStones = data[0].Split(' ').ToList();
            return ProcessStonesUsingMap(75, initialStones);
        }

        private long ProcessStones(int blinkCount, List<string> stones)
        {
            for (int blinks = 0; blinks < blinkCount; blinks++)
            {
                stones = ProcessStones(stones);
            }
            return stones.Count;
        }
        
        private List<string> ProcessStones(List<string> stones)
        {
            List<string> newStones = [];
            for (int i = 0; i < stones.Count; i++)
            {
                string currentStoneNumber = stones[i];
                if (currentStoneNumber == "0")
                {
                    newStones.Add("1");
                }
                else
                {
                    if (currentStoneNumber.Length % 2 == 0)
                    {
                        newStones.Add(long.Parse(currentStoneNumber[..(currentStoneNumber.Length / 2)]).ToString());
                        newStones.Add(long.Parse(currentStoneNumber[(currentStoneNumber.Length / 2)..]).ToString());
                    }
                    else
                    {
                        newStones.Add((long.Parse(currentStoneNumber) * 2024).ToString());
                    }
                }
            }
            return newStones;
        }

        private long ProcessStonesUsingMap(int blinkCount, List<string> stones)
        {
            Dictionary<string, long> stonesMap = new Dictionary<string, long>();
            foreach(var stone in stones)
            {
                if (stonesMap.ContainsKey(stone))
                {
                    stonesMap[stone] += 1;
                }
                else
                {
                    stonesMap.Add(stone, 1);
                }
            }

            for (int blinks = 0; blinks < blinkCount; blinks++)
            {
                Dictionary<string, long> newMap = new Dictionary<string, long>();
                foreach(var key in stonesMap.Keys)
                {
                    long count = stonesMap[key];
                    if (key == "0")
                    {
                        AddToMap(newMap, "1", count);
                    }
                    else
                    {
                        if (key.Length % 2 == 0)
                        {
                            AddToMap(newMap, (long.Parse(key[..(key.Length / 2)]).ToString()), count);
                            AddToMap(newMap, (long.Parse(key[(key.Length / 2)..]).ToString()), count);
                        }
                        else
                        {
                            AddToMap(newMap, (long.Parse(key) * 2024).ToString(), count);
                        }
                    }
                }
                stonesMap = newMap.ToDictionary(entry => entry.Key, entry => entry.Value);
            }
            return stonesMap.Select(s => s.Value).Sum();
        }

        private void AddToMap(Dictionary<string, long> map, string key, long count)
        {
            if (map.ContainsKey(key))
            {
                map[key] += count;
            }
            else
            {
                map.Add(key, count);
            }
        }
    }
}
