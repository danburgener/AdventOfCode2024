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
                var variations = new Combinatorics.Collections.Variations<int>(Enumerable.Range(0, antennasWithFrequency.Count()), 2);
                foreach (var variation in variations)
                {
                    antinodes.AddRange(GetAntinodes(antennasWithFrequency.ElementAt(variation[0]), antennasWithFrequency.ElementAt(variation[1]), rowMax, colMax));
                }
            }
            return antinodes.Distinct().Count();
        }

        private static List<string> GetAntinodes(Antenna antennaA, Antenna antennaB, int rowMax, int colMax)
        {
            List<string> antinodes = new List<string>();

            var rowDiff = Math.Abs(antennaA.Row - antennaB.Row);
            var colDiff = Math.Abs(antennaA.Column - antennaB.Column);

            if (antennaA.Row < antennaB.Row) //A is above B
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {
                    if (antennaA.Row - rowDiff >= 0 && antennaA.Column - colDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaA.Row - rowDiff, antennaA.Column - colDiff));
                    }
                    if (antennaB.Row + rowDiff <= rowMax && antennaB.Column + colDiff <= colMax)
                    {
                        antinodes.Add(GetKey(antennaB.Row + rowDiff, antennaB.Column + colDiff));
                    }
                }
                else if (antennaA.Column > antennaB.Column) //A is right B
                {
                    if (antennaA.Row - rowDiff >= 0 && antennaA.Column + colDiff <= colMax)
                    {
                        antinodes.Add(GetKey(antennaA.Row - rowDiff, antennaA.Column + colDiff));
                    }

                    if (antennaB.Row + rowDiff <= rowMax && antennaB.Column - colDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaB.Row + rowDiff, antennaB.Column - colDiff));
                    }
                }
                else //A and B are to the same level
                {

                    if (antennaA.Row - rowDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaA.Row - rowDiff, antennaA.Column));
                    }

                    if (antennaB.Row + rowDiff <= rowMax)
                    {
                        antinodes.Add(GetKey(antennaB.Row + rowDiff, antennaB.Column));
                    }
                }
            }
            else if (antennaA.Row > antennaB.Row) //A is below B
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {

                    if (antennaA.Row + rowDiff <= rowMax && antennaA.Column - colDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaA.Row + rowDiff, antennaA.Column - colDiff));
                    }

                    if (antennaB.Row - rowDiff >= 0 && antennaB.Column + colDiff <= colMax)
                    {
                        antinodes.Add(GetKey(antennaB.Row - rowDiff, antennaB.Column + colDiff));
                    }
                }
                else if (antennaA.Column > antennaB.Column) //A is right B
                {

                    if (antennaA.Row + rowDiff <= rowMax && antennaA.Column + colDiff <= colMax)
                    {
                        antinodes.Add(GetKey(antennaA.Row + rowDiff, antennaA.Column + colDiff));
                    }

                    if (antennaB.Row - rowDiff >= 0 && antennaB.Column - colDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaB.Row - rowDiff, antennaB.Column - colDiff));
                    }
                }
                else //A and B are to the same level
                {

                    if (antennaA.Row + rowDiff <= rowMax)
                    {
                        antinodes.Add(GetKey(antennaA.Row + rowDiff, antennaA.Column));
                    }

                    if (antennaB.Row - rowDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaB.Row - rowDiff, antennaB.Column));
                    }
                }
            }
            else //A and B are to the same level
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {

                    if (antennaA.Column - colDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaA.Row, antennaA.Column - colDiff));
                    }

                    if (antennaB.Column + colDiff <= colMax)
                    {
                        antinodes.Add(GetKey(antennaB.Row, antennaB.Column + colDiff));
                    }
                }
                else //A is right B
                {

                    if (antennaA.Column + colDiff <= colMax)
                    {
                        antinodes.Add(GetKey(antennaA.Row, antennaA.Column + colDiff));
                    }

                    if (antennaB.Column - colDiff >= 0)
                    {
                        antinodes.Add(GetKey(antennaB.Row, antennaB.Column - colDiff));
                    }
                }
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
                            Row = row,
                            Column = column,
                            Frequency = data[row][column]
                        });
                    }
                }
            }
            return antennas;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        public class Antenna
        {
            public char Frequency { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
        }

        private static string GetKey(int row, int col)
        {
            return $"{row}-{col}";
        }
    }
}
