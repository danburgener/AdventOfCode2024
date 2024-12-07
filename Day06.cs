using System.Diagnostics;

namespace AdventOfCode2024
{
    public class Day06 : IDay
    {
        private readonly string _fileDayName = "Six";
        public string GetName() => "Day 06";
        private const char Up = '^';
        private const char Down = 'v';
        private const char Left = '<';
        private const char Right = '>';
        private const char Obstruction = '#';
        private const char CurrentNewObstruction = 'O';
        private const char PastNewObstruction = 'X';
        private const char Visited = '|';
        private const int DirUp = 0;
        private const int DirDown = 1;
        private const int DirLeft = 2;
        private const int DirRight = 3;

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<string> locationsTraveled = new List<string>();
            var (currentCol, currentRow, currentDir) = GetStartingInfo(data);
            locationsTraveled.Add($"{currentRow}-{currentCol}");
            while (true)
            {
                var newInfo = currentDir switch
                {
                    DirUp => MoveUp(currentRow, currentCol, data),
                    DirDown => MoveDown(currentRow, currentCol, data),
                    DirLeft => MoveLeft(currentRow, currentCol, data),
                    DirRight => MoveRight(currentRow, currentCol, data),
                    _ => throw new NotImplementedException()
                };
                if (newInfo.newDirection == -1)
                {
                    break;
                }
                else if (newInfo.newRow != currentRow || newInfo.newCol != currentCol)
                {
                    currentRow = newInfo.newRow;
                    currentCol = newInfo.newCol;
                    locationsTraveled.Add($"{currentRow}-{currentCol}");
                }
                else
                {
                    currentDir = newInfo.newDirection;
                }
            }
            return locationsTraveled.Distinct().Count();
        }

        private char GetDirChar(int dir)
        {
            return dir switch
            {
                DirUp => Up,
                DirDown => Down,
                DirLeft => Left,
                DirRight => Right,
                _ => throw new NotImplementedException()
            };
        }

        public async Task<long> Two()
        {
            //Check current location and direction
            //If place item directly in front of the guard, what happens?
            //Does it eventually get into a loop?
            //Check for loop if guard returns to a previous location going same direction
            var data = await Common.ReadFile(_fileDayName, "Two");
            Dictionary<string, List<char>> locationTypes = new Dictionary<string, List<char>>();
            List<string> locationsTraveled = new List<string>();
            var (currentCol, currentRow, currentDir) = GetStartingInfo(data);
            locationTypes.Add($"{currentRow}-{currentCol}", new List<char> { GetDirChar(currentDir) });
            while (true)
            {
                var newInfo = currentDir switch
                {
                    DirUp => MoveUp(currentRow, currentCol, data),
                    DirDown => MoveDown(currentRow, currentCol, data),
                    DirLeft => MoveLeft(currentRow, currentCol, data),
                    DirRight => MoveRight(currentRow, currentCol, data),
                    _ => throw new NotImplementedException()
                };
                if (newInfo.newDirection == -1)
                {
                    break;
                }
                else if (newInfo.newRow != currentRow || newInfo.newCol != currentCol)
                {
                    currentRow = newInfo.newRow;
                    currentCol = newInfo.newCol;
                    locationsTraveled.Add($"{currentCol}-{currentRow}");
                }
                else
                {
                    currentDir = newInfo.newDirection;
                }
            }
            return locationsTraveled.Distinct().Count();
        }

        private static (int currentCol, int currentRow, int currentDir) GetStartingInfo(string[] data)
        {
            int currentCol = -1;
            int currentRow = -1;
            int currentDir = -1;
            for (var row = 0; row < data.Length; row++)
            {
                for (var col = 0; col < data[0].Length; col++)
                {
                    if (data[row][col] == Up)
                    {
                        currentDir = DirUp;
                        currentCol = col;
                        currentRow = row;
                        break;
                    }
                    else if (data[row][col] == Down)
                    {
                        currentDir = DirDown;
                        currentCol = col;
                        currentRow = row;
                        break;
                    }
                    else if (data[row][col] == Left)
                    {
                        currentDir = DirLeft;
                        currentCol = col;
                        currentRow = row;
                        break;
                    }
                    else if (data[row][col] == Right)
                    {
                        currentDir = DirRight;
                        currentCol = col;
                        currentRow = row;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (currentDir != -1)
                {
                    break;
                }
            }
            return (currentCol, currentRow, currentDir);
        }

        private (int newRow, int newCol, int newDirection) MoveUp(int currentRow, int currentCol, string[] data)
        {
            if (currentRow == 0)
            {
                return (-1, -1, -1);
            }
            if (data[currentRow-1][currentCol] == Obstruction)
            {
                return (currentRow, currentCol, DirRight);
            }
            return (currentRow-1, currentCol, DirUp);
        }

        private (int newRow, int newCol, int newDirection) MoveDown(int currentRow, int currentCol, string[] data)
        {
            if (currentRow == data.Length-1)
            {
                return (-1, -1, -1);
            }
            if (data[currentRow + 1][currentCol] == Obstruction)
            {
                return (currentRow, currentCol, DirLeft);
            }
            return (currentRow + 1, currentCol, DirDown);
        }

        private (int newRow, int newCol, int newDirection) MoveLeft(int currentRow, int currentCol, string[] data)
        {
            if (currentCol == 0)
            {
                return (-1, -1, -1);
            }
            if (data[currentRow][currentCol-1] == Obstruction)
            {
                return (currentRow, currentCol, DirUp);
            }
            return (currentRow, currentCol-1, DirLeft);
        }

        private (int newRow, int newCol, int newDirection) MoveRight(int currentRow, int currentCol, string[] data)
        {
            if (currentCol == data[0].Length - 1)
            {
                return (-1, -1, -1);
            }
            if (data[currentRow][currentCol+1] == Obstruction)
            {
                return (currentRow, currentCol, DirDown);
            }
            return (currentRow, currentCol+1, DirDown);
        }
    }
}
