
using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    public static class Day04
    {
        private static string MAS = "MAS";
        private static string XMAS = "XMAS";
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Four", "One");
            return FindXMas(data);
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Four", "Two");
            int count = 0;
            for (var rowIndex = 0; rowIndex < data.Length; rowIndex++)
            {
                for (var colIndex = 0; colIndex < data[0].Length; colIndex++)
                {
                    bool valid = CheckUp(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                    valid = CheckDown(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                    valid = CheckRight(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                    valid = CheckLeft(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                    valid = CheckUpLeft(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                    valid = CheckUpRight(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                    valid = CheckDownRight(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                    valid = CheckDownLeft(data, rowIndex, colIndex, XMAS);
                    if (valid) count++;
                }
            }
            return count;
        }

        private static bool CheckUpRight(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (colIndex + messageToCheck.Length >= data[0].Length || rowIndex - messageToCheck.Length < 0)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex - i][colIndex + i];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckDownRight(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (colIndex + messageToCheck.Length >= data[0].Length || rowIndex + messageToCheck.Length >= data.Length)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex + i][colIndex + i];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckDownLeft(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (colIndex - messageToCheck.Length < 0 || rowIndex + messageToCheck.Length >= data.Length)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex + i][colIndex - i];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckUpLeft(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (colIndex - messageToCheck.Length < 0 || rowIndex - messageToCheck.Length < 0)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex - i][colIndex - i];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckLeft(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (colIndex - messageToCheck.Length < 0)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex][colIndex - i];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckRight(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (colIndex + messageToCheck.Length >= data.Length)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex][colIndex + i];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckDown(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (rowIndex + messageToCheck.Length >= data[0].Length)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex + 1][colIndex];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckUp(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (rowIndex - messageToCheck.Length < 0)
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex - 1][colIndex];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static long FindXMas(string[] data)
        {
            int count = 0;

            List<string> list = data.ToList();

            string regexString = "(?=XMAS)|(?=SAMX)";
            System.Text.RegularExpressions.Regex regex = new(regexString);
            count = GetRowCount(count, list, regex);

            count = GetColumnCount(count, list, regex);


            count = GetDiagonalCount(count, list, regex);

            return count;
        }

        private static int GetRowCount(int count, List<string> list, Regex regex)
        {
            for (int rowIndex = 0; rowIndex < list.Count; rowIndex++)
            {
                var matches = regex.Matches(list[rowIndex]);


                if (matches.Count > 0)
                {
                    count += matches.Count;
                    Console.WriteLine($"Row:{rowIndex} - Count {count} - {list[rowIndex]}");
                }
                else
                {
                    Console.WriteLine($"Row:{rowIndex} - {list[rowIndex]}");

                }
            }

            return count;
        }

        private static int GetColumnCount(int count, List<string> list, Regex regex)
        {
            for (int columnIndex = 0; columnIndex < list[0].Length; columnIndex++)
            {
                string line = "";
                for (int rowIndex = 0; rowIndex < list.Count; rowIndex++)
                {
                    line += list[rowIndex][columnIndex];
                }
                var matches = regex.Matches(line);

                if (matches.Count > 0)
                {
                    count += matches.Count;
                    Console.WriteLine($"Column:{columnIndex} - Count {count} - {line}");
                }
                else
                {

                    Console.WriteLine($"Column:{columnIndex} - {line}");
                }
            }

            return count;
        }

        private static int GetDiagonalCount(int count, List<string> list, Regex regex)
        {
            for (int rowIndex = 0; rowIndex < list.Count; rowIndex++)
            {
                var diagonalString = GetDiagonal(list, rowIndex, 0);
                var matches = regex.Matches(diagonalString);
                if (matches.Count > 0)
                {
                    count += matches.Count;
                    Console.WriteLine($"Row:{rowIndex} Column:{0} - Count {count} - {diagonalString}");
                }
                else
                {

                    Console.WriteLine($"Row:{rowIndex} Column:{0} - {diagonalString}");
                }
            }

            for (int columnIndex = 1; columnIndex < list[0].Length; columnIndex++)
            {
                var diagonalString = GetDiagonal(list, 0, columnIndex);
                var matches = regex.Matches(diagonalString);
                if (matches.Count > 0)
                {
                    count += matches.Count;
                    Console.WriteLine($"Row:{0} Column:{columnIndex} - Count {count} - {diagonalString}");
                }
                else
                {

                    Console.WriteLine($"Row:{0} Column:{columnIndex} - {diagonalString}");
                }
            }

            for (int rowIndex = 0; rowIndex < list.Count; rowIndex++)
            {
                var diagonalString = GetDiagonalBack(list, rowIndex, list[rowIndex].Length - 1);
                var matches = regex.Matches(diagonalString);
                if (matches.Count > 0)
                {
                    count += matches.Count;
                    Console.WriteLine($"Row:{rowIndex} Column:{list[rowIndex].Length - 1} - Count {count} - {diagonalString}");
                }
                else
                {

                    Console.WriteLine($"Row:{rowIndex} Column:{list[rowIndex].Length - 1} - {diagonalString}");
                }
            }

            for (int columnIndex = 0; columnIndex < list[0].Length - 1; columnIndex++)
            {
                var diagonalString = GetDiagonalBack(list, 0, columnIndex);
                var matches = regex.Matches(diagonalString);
                if (matches.Count > 0)
                {
                    count += matches.Count;
                    Console.WriteLine($"Row:{0} Column:{columnIndex} - Count {count} - {diagonalString}");
                }
                else
                {

                    Console.WriteLine($"Row:{0} Column:{columnIndex} - {diagonalString}");
                }
            }

            return count;
        }

        private static string GetDiagonal(List<string> list, int startRow, int startColumn)
        {
            int column = startColumn;
            List<char> chars = new List<char>();
            for (var rowIndex = startRow; rowIndex < list.Count; rowIndex++)
            {
                if (column < list[rowIndex].Length)
                {
                    chars.Add(list[rowIndex][column]);
                    column++;
                }
                else
                {
                    break;
                }
            }
            return string.Join("", chars);
        }

        private static string GetDiagonalBack(List<string> list, int startRow, int startColumn)
        {
            int column = startColumn;
            List<char> chars = new List<char>();
            for (var rowIndex = startRow; rowIndex < list.Count; rowIndex++)
            {
                if (column >= 0)
                {
                    chars.Add(list[rowIndex][column]);
                    column--;
                }
                else
                {
                    break;
                }
            }
            return string.Join("", chars);
        }
    }
}
