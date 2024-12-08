
namespace AdventOfCode2024
{
    public class Day08 : IDay
    {
        private readonly string _fileDayName = "Eight";
        public string GetName() => "Day 08";

        private enum Directions
        {
            DownRight,
            DownLeft,
            Down,
            Right
        }

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
                antinodes.Add(GetKey(antenna.Row, antenna.Column));
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

            if (antennaA.Row < antennaB.Row) //A is above B
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {
                    antinodes.AddRange(GetAntinodes(antennaA, antennaB, rowMax, colMax, Directions.DownRight));
                }
                else if (antennaA.Column > antennaB.Column) //A is right B
                {
                    antinodes.AddRange(GetAntinodes(antennaA, antennaB, rowMax, colMax, Directions.DownLeft));
                }
                else //A and B are to the same level
                {
                    antinodes.AddRange(GetAntinodes(antennaA, antennaB, rowMax, colMax, Directions.Down));
                }
            }
            else if (antennaA.Row > antennaB.Row) //A is below B
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {
                    antinodes.AddRange(GetAntinodes(antennaB, antennaA, rowMax, colMax, Directions.DownLeft));
                }
                else if (antennaA.Column > antennaB.Column) //A is right B
                {
                    antinodes.AddRange(GetAntinodes(antennaB, antennaA, rowMax, colMax, Directions.DownRight));
                }
                else //A and B are to the same level
                {
                    antinodes.AddRange(GetAntinodes(antennaB, antennaA, rowMax, colMax, Directions.Down));
                }
            }
            else //A and B are to the same level
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {
                    antinodes.AddRange(GetAntinodes(antennaA, antennaB, rowMax, colMax, Directions.Right));
                }
                else //A is right B
                {
                    antinodes.AddRange(GetAntinodes(antennaB, antennaA, rowMax, colMax, Directions.Right));
                }
            }
            return antinodes;
        }

        private static List<string> GetAntinodesWithResonant(Antenna antennaA, Antenna antennaB, int rowMax, int colMax)
        {
            List<string> antinodes = new List<string>();

            if (antennaA.Row < antennaB.Row) //A is above B
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaA, antennaB, rowMax, colMax, Directions.DownRight));
                }
                else if (antennaA.Column > antennaB.Column) //A is right B
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaA, antennaB, rowMax, colMax, Directions.DownLeft));
                }
                else //A and B are to the same level
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaA, antennaB, rowMax, colMax, Directions.Down));
                }
            }
            else if (antennaA.Row > antennaB.Row) //A is below B
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaB, antennaA, rowMax, colMax, Directions.DownLeft));
                }
                else if (antennaA.Column > antennaB.Column) //A is right B
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaB, antennaA, rowMax, colMax, Directions.DownRight));
                }
                else //A and B are to the same level
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaB, antennaA, rowMax, colMax, Directions.Down));
                }
            }
            else //A and B are to the same level
            {
                if (antennaA.Column < antennaB.Column) //A is left B
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaA, antennaB, rowMax, colMax, Directions.Right));
                }
                else //A is right B
                {
                    antinodes.AddRange(GetAntinodesWithResonant(antennaB, antennaA, rowMax, colMax, Directions.Right));
                }
            }
            return antinodes;
        }

        private static IEnumerable<string> GetAntinodes(Antenna antennaA, Antenna antennaB, int rowMax, int colMax, Directions direction)
        {
            var rowDiff = Math.Abs(antennaA.Row - antennaB.Row);
            var colDiff = Math.Abs(antennaA.Column - antennaB.Column);

            List<string> antinodes = new List<string>();

            switch (direction)
            {
                case Directions.DownRight:
                    {
                        if (antennaA.Row - rowDiff >= 0 && antennaA.Column - colDiff >= 0)
                        {
                            antinodes.Add(GetKey(antennaA.Row - rowDiff, antennaA.Column - colDiff));
                        }
                        if (antennaB.Row + rowDiff <= rowMax && antennaB.Column + colDiff <= colMax)
                        {
                            antinodes.Add(GetKey(antennaB.Row + rowDiff, antennaB.Column + colDiff));
                        }
                        break;
                    }
                case Directions.DownLeft:
                    {
                        if (antennaA.Row - rowDiff >= 0 && antennaA.Column + colDiff <= colMax)
                        {
                            antinodes.Add(GetKey(antennaA.Row - rowDiff, antennaA.Column + colDiff));
                        }

                        if (antennaB.Row + rowDiff <= rowMax && antennaB.Column - colDiff >= 0)
                        {
                            antinodes.Add(GetKey(antennaB.Row + rowDiff, antennaB.Column - colDiff));
                        }
                        break;
                    }
                case Directions.Right:
                    {
                        if (antennaA.Column - colDiff >= 0)
                        {
                            antinodes.Add(GetKey(antennaA.Row, antennaA.Column - colDiff));
                        }

                        if (antennaB.Column + colDiff <= colMax)
                        {
                            antinodes.Add(GetKey(antennaB.Row, antennaB.Column + colDiff));
                        }
                        break;
                    }
                case Directions.Down:
                    {
                        if (antennaA.Row - rowDiff >= 0)
                        {
                            antinodes.Add(GetKey(antennaA.Row - rowDiff, antennaA.Column));
                        }

                        if (antennaB.Row + rowDiff <= rowMax)
                        {
                            antinodes.Add(GetKey(antennaB.Row + rowDiff, antennaB.Column));
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            return antinodes;
        }

        private static IEnumerable<string> GetAntinodesWithResonant(Antenna antennaA, Antenna antennaB, int rowMax, int colMax, Directions direction)
        {
            var rowDiff = Math.Abs(antennaA.Row - antennaB.Row);
            var colDiff = Math.Abs(antennaA.Column - antennaB.Column);

            List<string> antinodes = new List<string>();

            switch (direction)
            {
                case Directions.DownRight:
                    {
                        int currentRow = antennaA.Row;
                        int currentCol = antennaA.Column;
                        while(currentRow - rowDiff >= 0 && currentCol - colDiff >= 0)
                        {
                            antinodes.Add(GetKey(currentRow - rowDiff, currentCol - colDiff));
                            currentRow -= rowDiff;
                            currentCol -= colDiff;
                        }
                        currentRow = antennaB.Row;
                        currentCol = antennaB.Column;
                        while (currentRow + rowDiff <= rowMax && currentCol + colDiff <= colMax)
                        {
                            antinodes.Add(GetKey(currentRow + rowDiff, currentCol + colDiff));
                            currentRow += rowDiff;
                            currentCol += colDiff;
                        }
                        break;
                    }
                case Directions.DownLeft:
                    {
                        int currentRow = antennaA.Row;
                        int currentCol = antennaA.Column;
                        while (currentRow - rowDiff >= 0 && currentCol + colDiff <= colMax)
                        {
                            antinodes.Add(GetKey(currentRow - rowDiff, currentCol + colDiff));
                            currentRow -= rowDiff;
                            currentCol += colDiff;
                        }

                        currentRow = antennaB.Row;
                        currentCol = antennaB.Column;
                        while (currentRow + rowDiff <= rowMax && currentCol - colDiff >= 0)
                        {
                            antinodes.Add(GetKey(currentRow + rowDiff, currentCol - colDiff));
                            currentRow += rowDiff;
                            currentCol -= colDiff;
                        }
                        break;
                    }
                case Directions.Right:
                    {
                        int currentCol = antennaA.Column;
                        while (currentCol - colDiff >= 0)
                        {
                            antinodes.Add(GetKey(antennaA.Row, currentCol - colDiff));
                            currentCol -= colDiff;
                        }

                        currentCol = antennaB.Column;
                        while (currentCol + colDiff <= colMax)
                        {
                            antinodes.Add(GetKey(antennaB.Row, currentCol + colDiff));
                            currentCol += colDiff;
                        }
                        break;
                    }
                case Directions.Down:
                    {
                        int currentRow = antennaA.Row;
                        while (currentRow - rowDiff >= 0)
                        {
                            antinodes.Add(GetKey(currentRow - rowDiff, antennaA.Column));
                            currentRow -= rowDiff;
                        }

                        currentRow = antennaB.Row;
                        while (currentRow + rowDiff <= rowMax)
                        {
                            antinodes.Add(GetKey(currentRow + rowDiff, antennaB.Column));
                            currentRow += rowDiff;
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
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
