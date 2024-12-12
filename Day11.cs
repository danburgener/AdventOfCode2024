
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

        private long ProcessStones(int blinkCount, List<string> stones)
        {
            for (int blinks = 0; blinks < blinkCount; blinks++)
            {
                stones = ProcessStones(stones);
            }
            return stones.Count;
        }

        private long ProcessStonesChunked(List<string> stones)
        {
            List<List<string>> runningChunks = new List<List<string>>();
            List<List<string>> allChunks = new List<List<string>>();
            Console.WriteLine("Step 10");
            runningChunks = ProcessStonesChunked(10, stones);
            Console.WriteLine("Step 20");
            foreach (var chunk in runningChunks)
            {
                allChunks.AddRange(ProcessStonesChunked(10, chunk));
            }
            runningChunks = allChunks.Select(a => a.ToList()).ToList();
            allChunks = new List<List<string>>();
            Console.WriteLine("Step 30");
            foreach (var chunk in runningChunks)
            {
                allChunks.AddRange(ProcessStonesChunked(10, chunk));
            }
            runningChunks = allChunks.Select(a => a.ToList()).ToList();
            allChunks = new List<List<string>>();
            Console.WriteLine("Step 40");
            foreach (var chunk in runningChunks)
            {
                allChunks.AddRange(ProcessStonesChunked(10, chunk));
            }
            runningChunks = allChunks.Select(a => a.ToList()).ToList();
            allChunks = new List<List<string>>();
            Console.WriteLine("Step 50");
            foreach (var chunk in runningChunks)
            {
                allChunks.AddRange(ProcessStonesChunked(10, chunk));
            }
            runningChunks = allChunks.Select(a => a.ToList()).ToList();
            allChunks = new List<List<string>>();
            Console.WriteLine("Step 60");
            foreach (var chunk in runningChunks)
            {
                allChunks.AddRange(ProcessStonesChunked(10, chunk));
            }
            runningChunks = allChunks.Select(a => a.ToList()).ToList();
            allChunks = new List<List<string>>();
            Console.WriteLine("Step 70");
            foreach (var chunk in runningChunks)
            {
                allChunks.AddRange(ProcessStonesChunked(10, chunk));
            }
            runningChunks = allChunks.Select(a => a.ToList()).ToList();
            allChunks = new List<List<string>>();
            Console.WriteLine("Step 75");
            foreach (var chunk in runningChunks)
            {
                allChunks.AddRange(ProcessStonesChunked(5, chunk));
            }
            return allChunks.Select(a => a.Count()).Sum();
        }

        private List<List<string>> ProcessStonesChunked(int blinkCount, List<string> stones)
        {
            int chunkSize = 500;
            List<List<string>> chunks = stones.Chunk(chunkSize).Select(c => c.ToList()).ToList();
            for (int blinks = 0; blinks < blinkCount; blinks++)
            {
                for (var i = 0; i < chunks.Count; i++)
                {
                    chunks[i] = ProcessStones(chunks[i]);
                }
            }
            return chunks;
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

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            var initialStones = data[0].Split(' ').ToList();
            long count = 0;
            List<Task<long>> tasks = new List<Task<long>>();
            foreach(var stone in initialStones)
            {
                tasks.Add(Task.Run(() => ProcessStonesChunked(new List<string> { stone })));
            }
            var results = await Task.WhenAll(tasks);
            return results.Sum();
        }
    }
}
