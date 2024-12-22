namespace AdventOfCode2024
{
    public class Day22 : IDay
    {
        private readonly string _fileDayName = "TwentyTwo";
        public string GetName() => "Day 22";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            var numbers = data.Select(d => long.Parse(d));
            long total = 0;
            foreach(var number in numbers)
            {
                var secretNumber = number;
                for (int i = 0; i < 2000; i++)
                {
                    secretNumber = ProcessNumber(secretNumber);
                }
                total += secretNumber;
            }

            return total;
        }

        public long ProcessNumber(long secretNumber)
        {
            var resultOne = secretNumber * 64;
            secretNumber = Mix(secretNumber, resultOne);
            secretNumber = Prune(secretNumber);

            int resultTwo = (int)(secretNumber / 32);
            secretNumber = Mix(secretNumber, resultTwo);
            secretNumber = Prune(secretNumber);

            var resultThree = secretNumber * 2048;
            secretNumber = Mix(secretNumber, resultThree);
            secretNumber = Prune(secretNumber);
            return secretNumber;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        private long Mix(long secretNumber, long value)
        {
            return value ^ secretNumber;
        }

        private long Prune(long secretNumber)
        {
            return secretNumber % 16777216;
        }
    }
}
