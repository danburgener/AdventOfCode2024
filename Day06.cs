using System.Text;

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
        private const char EmptySpace = '.';
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
            var data = await Common.ReadFile(_fileDayName, "Two");
            //int loops = GetLoops(data);
            int loops = GetLoopsBruteForce(data);
            return loops;
        }

        private int GetLoops(string[] data)
        {
            var (startingCol, startingRow, currentDir) = GetStartingInfo(data);
            int currentCol = startingCol;
            int currentRow = startingRow;
            List<string> loops = new();

            while (true)
            {
                var dataToTest = data.ToArray();
                var loopInfo = GetLoopKey(currentRow, currentCol, currentDir, dataToTest, startingRow, startingCol);
                if (!string.IsNullOrEmpty(loopInfo))
                {
                    loops.Add(loopInfo);
                }
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
                }
                else
                {
                    currentDir = newInfo.newDirection;
                }
            }
            return loops.Distinct().Count();
        }

        private int GetLoopsBruteForce(string[] data)
        {
            var (startingCol, startingRow, startingDir) = GetStartingInfo(data);
            int loops = 0;
            for(int row = 0; row < data.Length; row++)
            {
                for(int col = 0; col < data[0].Length; col++)
                {
                    if (data[row][col] == EmptySpace)
                    {
                        var newData = data.ToArray();
                        newData[row] = newData[row].Replace(col, Obstruction);
                        if(IsLoop(startingRow, startingCol, startingDir, newData))
                        {
                            loops++;
                        }
                    }
                }
            }
            return loops;
        }

        private bool IsLoop(int currentRow, int currentCol, int currentDir, string[] data)
        {
            Dictionary<string, List<char>> locationTypes = new Dictionary<string, List<char>>();
            locationTypes.Add(GetKey(currentRow, currentCol), new List<char> { GetDirChar(currentDir) });

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
                    return false;
                }
                else
                {
                    var newInfoKey = GetKey(newInfo.newRow, newInfo.newCol);
                    char newDirectionChar = GetDirChar(newInfo.newDirection);

                    if (!locationTypes.ContainsKey(newInfoKey))
                    {
                        locationTypes.Add(newInfoKey, new List<char>());
                    }

                    if (locationTypes[newInfoKey].Contains(newDirectionChar))
                    {
                        return true;
                    }

                    locationTypes[newInfoKey].Add(newDirectionChar);

                    if (newInfo.newRow != currentRow || newInfo.newCol != currentCol)
                    {
                        currentRow = newInfo.newRow;
                        currentCol = newInfo.newCol;
                    }
                    else
                    {
                        currentDir = newInfo.newDirection;
                    }
                }
            }
        }

        private string GetLoopKey(int currentRow, int currentCol, int currentDir, string[] data, int startingRow, int startingCol)
        {
            string obstructionLocation = string.Empty;
            #region Setup New Obstruction, return if not possible
            if (currentDir == DirDown)
            {
                if (currentRow == data.Length - 1 ||
                    (currentRow + 1 == startingRow && currentCol == startingCol) ||
                    data[currentRow + 1][currentCol] == Obstruction)
                {
                    return string.Empty;
                }
                else
                {
                    obstructionLocation = GetKey(currentRow + 1, currentCol);
                    data[currentRow + 1] = data[currentRow + 1].Replace(currentCol, Obstruction);
                }
            }
            else if (currentDir == DirUp)
            {
                if (currentRow == 0 ||
                    (currentRow - 1 == startingRow && currentCol == startingCol) ||
                    data[currentRow - 1][currentCol] == Obstruction)
                {
                    return string.Empty;
                }
                else
                {
                    obstructionLocation = GetKey(currentRow - 1, currentCol);
                    data[currentRow - 1] = data[currentRow - 1].Replace(currentCol, Obstruction);
                }
            }
            else if (currentDir == DirLeft)
            {
                if (currentCol == 0 ||
                    (currentRow == startingRow && currentCol - 1 == startingCol) ||
                     data[currentRow][currentCol - 1] == Obstruction)
                {
                    return string.Empty;
                }
                else
                {
                    obstructionLocation = GetKey(currentRow, currentCol - 1);
                    data[currentRow] = data[currentRow].Replace(currentCol - 1, Obstruction);
                }
            }
            else if (currentDir == DirRight)
            {
                if (currentCol == data[0].Length - 1 ||
                    (currentRow == startingRow && currentCol + 1 == startingCol) ||
                      data[currentRow][currentCol + 1] == Obstruction)
                {
                    return string.Empty;
                }
                else
                {
                    obstructionLocation = GetKey(currentRow, currentCol + 1);
                    data[currentRow] = data[currentRow].Replace(currentCol + 1, Obstruction);
                }
            }
            #endregion

            Dictionary<string, List<char>> locationTypes = new Dictionary<string, List<char>>();
            locationTypes.Add(GetKey(currentRow, currentCol), new List<char> { GetDirChar(currentDir) });

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
                    return string.Empty;
                }
                else
                {
                    var newInfoKey = GetKey(newInfo.newRow, newInfo.newCol);
                    char newDirectionChar = GetDirChar(newInfo.newDirection);

                    if (!locationTypes.ContainsKey(newInfoKey))
                    {
                        locationTypes.Add(newInfoKey, new List<char>());
                    }

                    if (locationTypes[newInfoKey].Contains(newDirectionChar))
                    {
                        return obstructionLocation;
                    }

                    locationTypes[newInfoKey].Add(newDirectionChar);

                    if (newInfo.newRow != currentRow || newInfo.newCol != currentCol)
                    {
                        currentRow = newInfo.newRow;
                        currentCol = newInfo.newCol;
                    }
                    else
                    {
                        currentDir = newInfo.newDirection;
                    }
                }
            }
        }

        private static string GetKey(int row, int col)
        {
            return $"{row}-{col}";
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
            if (data[currentRow - 1][currentCol] == Obstruction)
            {
                return (currentRow, currentCol, DirRight);
            }
            return (currentRow - 1, currentCol, DirUp);
        }

        private (int newRow, int newCol, int newDirection) MoveDown(int currentRow, int currentCol, string[] data)
        {
            if (currentRow == data.Length - 1)
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
            if (data[currentRow][currentCol - 1] == Obstruction)
            {
                return (currentRow, currentCol, DirUp);
            }
            return (currentRow, currentCol - 1, DirLeft);
        }

        private (int newRow, int newCol, int newDirection) MoveRight(int currentRow, int currentCol, string[] data)
        {
            if (currentCol == data[0].Length - 1)
            {
                return (-1, -1, -1);
            }
            if (data[currentRow][currentCol + 1] == Obstruction)
            {
                return (currentRow, currentCol, DirDown);
            }
            return (currentRow, currentCol + 1, DirRight);
        }
    }
}
