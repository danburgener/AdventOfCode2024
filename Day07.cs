
namespace AdventOfCode2024
{
    public class Day07 : IDay
    {
        private readonly string _fileDayName = "Seven";
        public string GetName() => "Day 07";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            long count = 0;
            var characters = new List<char> { '*', '+' };
            foreach (var line in data)
            {
                long value = GetLineValue(line, characters);
                count += value;
            }
            return count;
        }

        private static long GetLineValue(string line, List<char> characters)
        {
            var parsedLine = line.Split(':');
            var testValue = long.Parse(parsedLine[0]);
            var numbers = parsedLine[1].Trim().Split(' ').Select(s => long.Parse(s));
            if (LineIsValid(testValue, numbers.ToList(), characters)){

                return testValue;
            }
            return 0;
        }

        private static bool LineIsValid(long testValue, List<long> numbers, List<char> characters)
        {
            if (testValue == numbers.Sum())
            {
                return true;
            }else if (testValue == numbers.Aggregate(1L, (a,b) => a * b))
            {
                return true;
            }
            else
            {
                var variations = new Combinatorics.Collections.Variations<char>(characters, numbers.Count-1, Combinatorics.Collections.GenerateOption.WithRepetition);
                foreach(var variation in variations)
                {
                    long total = numbers[0];

                    for (var i = 1; i < numbers.Count; i++)
                    {
                        long number = numbers[i];
                        char character = variation[i - 1];
                        if (character == '*')
                        {
                            total *= number;
                        }
                        else if(character == '+')
                        {
                            total += number;
                        }
                        else if (character == '|')
                        {
                            total = long.Parse($"{total}{number}");
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    if (total == testValue)
                    {
                        return true;
                    }
                }
                return false;

            }
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            long count = 0;
            var characters = new List<char> { '*', '+', '|' };
            foreach (var line in data)
            {
                long value = GetLineValue(line, characters);
                count += value;
            }
            return count;
        }
    }
}
