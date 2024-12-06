namespace AdventOfCode2024
{
    public class Day04 : IDay
    {
        public string GetName() => "Day 04";
        private static readonly string MAS = "MAS";
        private static readonly string XMAS = "XMAS";
        public async Task<long> One()
        {
            var data = await Common.ReadFile("Four", "One");
            int count = 0;
            for (var rowIndex = 0; rowIndex < data.Length; rowIndex++)
            {
                for (var colIndex = 0; colIndex < data[0].Length; colIndex++)
                {
                    if (CheckUp(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                    if (CheckDown(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                    if (CheckRight(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                    if (CheckLeft(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                    if (CheckUpLeft(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                    if (CheckUpRight(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                    if (CheckDownRight(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                    if (CheckDownLeft(data, rowIndex, colIndex, XMAS))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile("Four", "Two");
            int count = 0;
            List<string> alreadyFound = new();
            for (var rowIndex = 0; rowIndex < data.Length; rowIndex++)
            {
                for (var colIndex = 0; colIndex < data[0].Length; colIndex++)
                {
                    if (CheckUpLeft(data, rowIndex, colIndex, MAS) && (CheckUpRight(data, rowIndex, colIndex - 2, MAS) || CheckDownLeft(data, rowIndex - 2, colIndex, MAS)))
                    {
                        string middleOfX = $"{rowIndex - 1}-{colIndex - 1}";
                        if (!alreadyFound.Contains(middleOfX))
                        {
                            alreadyFound.Add(middleOfX);
                            count++;
                        }
                    }
                    if (CheckUpRight(data, rowIndex, colIndex, MAS) && (CheckUpLeft(data, rowIndex, colIndex + 2, MAS) || CheckDownRight(data, rowIndex - 2, colIndex, MAS)))
                    {
                        string middleOfX = $"{rowIndex - 1}-{colIndex + 1}";
                        if (!alreadyFound.Contains(middleOfX))
                        {
                            alreadyFound.Add(middleOfX);
                            count++;
                        }
                    }
                    if (CheckDownRight(data, rowIndex, colIndex, MAS) && (CheckDownLeft(data, rowIndex, colIndex + 2, MAS) || CheckUpRight(data, rowIndex + 2, colIndex, MAS)))
                    {
                        string middleOfX = $"{rowIndex + 1}-{colIndex + 1}";
                        if (!alreadyFound.Contains(middleOfX))
                        {
                            alreadyFound.Add(middleOfX);
                            count++;
                        }
                    }
                    if (CheckDownLeft(data, rowIndex, colIndex, MAS) && (CheckDownRight(data, rowIndex, colIndex - 2, MAS) || CheckUpLeft(data, rowIndex + 2, colIndex, MAS)))
                    {
                        string middleOfX = $"{rowIndex + 1}-{colIndex - 1}";
                        if (!alreadyFound.Contains(middleOfX))
                        {
                            alreadyFound.Add(middleOfX);
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private static bool CheckUpRight(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (TooCloseToUpperBounds(colIndex, messageToCheck, data[0].Length) || TooCloseToLowerBounds(rowIndex, messageToCheck))
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
            if (TooCloseToUpperBounds(colIndex, messageToCheck, data[0].Length) || TooCloseToUpperBounds(rowIndex, messageToCheck, data.Length))
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
            if (TooCloseToLowerBounds(colIndex, messageToCheck) || TooCloseToUpperBounds(rowIndex, messageToCheck, data.Length))
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
            if (TooCloseToLowerBounds(colIndex, messageToCheck) || TooCloseToLowerBounds(rowIndex, messageToCheck))
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
            if (TooCloseToLowerBounds(colIndex, messageToCheck))
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
            if (TooCloseToUpperBounds(colIndex, messageToCheck, data.Length))
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
            if (TooCloseToUpperBounds(rowIndex, messageToCheck, data.Length))
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex + i][colIndex];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool CheckUp(string[] data, int rowIndex, int colIndex, string messageToCheck)
        {
            if (TooCloseToLowerBounds(rowIndex, messageToCheck))
            {
                return false;
            }
            string message = "";
            for (var i = 0; i < messageToCheck.Length; i++)
            {
                message += data[rowIndex - i][colIndex];
            }
            if (messageToCheck == message)
            {
                return true;
            }
            return false;
        }

        private static bool TooCloseToLowerBounds(int index, string messageToCheck)
        {
            return index - (messageToCheck.Length - 1) < 0;
        }

        private static bool TooCloseToUpperBounds(int index, string messageToCheck, int arrayLength)
        {
            return index + messageToCheck.Length > arrayLength;
        }
    }
}
