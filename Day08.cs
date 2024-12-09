
namespace AdventOfCode2024
{
    public class Day08 : IDay
    {
        private readonly string _fileDayName = "Eight";
        public string GetName() => "Day 08";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<string> antinodes = new List<string>();
            List<Antenna> antennas = GetAntennas(data);
            var distinctFrequencies = antennas.Select(x => x.Frequency).Distinct();
            int rowMax = data.Length - 1;
            int colMax = data[0].Length - 1;
            foreach (var frequency in distinctFrequencies)
            {
                var antennasWithFrequency = antennas.Where(a => a.Frequency == frequency);
                var combinations = new Combinatorics.Collections.Combinations<int>(Enumerable.Range(0, antennasWithFrequency.Count()), 2);
                foreach (var combination in combinations)
                {
                    antinodes.AddRange(GetAntinodes(antennasWithFrequency.ElementAt(combination[0]), antennasWithFrequency.ElementAt(combination[1]), rowMax, colMax));
                }
            }
            return antinodes.Distinct().Count();
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            List<string> antinodes = new List<string>();
            List<Antenna> antennas = GetAntennas(data);
            var distinctFrequencies = antennas.Select(x => x.Frequency).Distinct();
            int rowMax = data.Length - 1;
            int colMax = data[0].Length - 1;
            foreach(var antenna in antennas) //Each antenna counts as a antinode
            {
                antinodes.Add(GetKey(antenna.Vector.Y, antenna.Vector.X));
            }
            foreach (var frequency in distinctFrequencies)
            {
                var antennasWithFrequency = antennas.Where(a => a.Frequency == frequency);
                var combinations = new Combinatorics.Collections.Combinations<int>(Enumerable.Range(0, antennasWithFrequency.Count()), 2);
                foreach (var combination in combinations)
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennasWithFrequency.ElementAt(combination[0]), antennasWithFrequency.ElementAt(combination[1]), rowMax, colMax));
                }
            }
            return antinodes.Distinct().Count();
        }

        private static List<string> GetAntinodes(Antenna antennaA, Antenna antennaB, int rowMax, int colMax)
        {
            List<string> antinodes = new List<string>();
            var newVector = antennaA.Vector - antennaB.Vector;
            var another = antennaA.Vector + newVector;
            if (another.X >= 0 && another.X <= colMax && another.Y >= 0 && another.Y <= rowMax)
            {
                antinodes.Add(GetKey((int)another.Y, (int)another.X));
            }
            var andAnother = antennaB.Vector - newVector;
            if (andAnother.X >= 0 && andAnother.X <= colMax && andAnother.Y >= 0 && andAnother.Y <= rowMax)
            {
                antinodes.Add(GetKey((int)andAnother.Y, (int)andAnother.X));
            }
            return antinodes;
        }

        private static List<string> GetAntinodesWithResonant(Antenna antennaA, Antenna antennaB, int rowMax, int colMax)
        {
            List<string> antinodes = new List<string>();
            var newVector = antennaA.Vector - antennaB.Vector;
            var another = antennaA.Vector + newVector;
            while (another.X >= 0 && another.X <= colMax && another.Y >= 0 && another.Y <= rowMax)
            {
                antinodes.Add(GetKey((int)another.Y, (int)another.X));
                another = another + newVector;
            }
            var andAnother = antennaB.Vector - newVector;
            while (andAnother.X >= 0 && andAnother.X <= colMax && andAnother.Y >= 0 && andAnother.Y <= rowMax)
            {
                antinodes.Add(GetKey((int)andAnother.Y, (int)andAnother.X));
                andAnother = andAnother - newVector;
            }
            return antinodes;
        }

        
        private List<Antenna> GetAntennas(string[] data)
        {
            List<Antenna> antennas = new List<Antenna>();
            for (int row = 0; row < data.Length; row++)
            {
                for (int column = 0; column < data[row].Length; column++)
                {
                    if (data[row][column] != '.')
                    {
                        antennas.Add(new Antenna
                        {
                            Vector = new System.Numerics.Vector2(column, row),
                            Frequency = data[row][column]
                        });
                    }
                }
            }
            return antennas;
        }

        public class Antenna
        {
            public char Frequency { get; set; }
            public System.Numerics.Vector2 Vector { get; set; }
        }

        private static string GetKey(float row, float col)
        {
            return $"{row}-{col}";
        }
    }
}
