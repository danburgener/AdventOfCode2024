

namespace AdventOfCode2024
{
    public class Day15 : IDay
    {
        private readonly string _fileDayName = "Fifteen";
        public string GetName() => "Day 15";
        private const char Wall = '#';
        private const char Box = 'O';
        private const char Robot = '@';
        private const char Empty = '.';

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            (List<List<char>> map, List<char> commands) = ParseData(data);
            (int currentRow, int currentColumn) = FindRobot(map);
            foreach (var command in commands)
            {
                switch (command)
                {
                    case '^':
                        (currentRow, currentColumn) = MoveUp(map, currentRow, currentColumn);
                        break;
                    case '>':
                        (currentRow, currentColumn) = MoveRight(map, currentRow, currentColumn);
                        break;
                    case 'v':
                        (currentRow, currentColumn) = MoveDown(map, currentRow, currentColumn);
                        break;
                    case '<':
                        (currentRow, currentColumn) = MoveLeft(map, currentRow, currentColumn);
                        break;
                    default:
                        break;
                };
            }
            int count = 0;
            return count;
        }

        private (int currentRow, int currentColumn) FindRobot(List<List<char>> map)
        {
            for (var row = 0; row < map.Count; row++)
            {
                var indexOfRobot = map[row].IndexOf(Robot);
                if (indexOfRobot != -1)
                {
                    return (row, indexOfRobot);
                }
            }
            return (-1, -1);
        }

        private (int currentRow, int currentColumn) MoveLeft(List<List<char>> map, int currentRow, int currentColumn)
        {
            if (map[currentRow][currentColumn - 1] == Empty)
            {
                map[currentRow][currentColumn - 1] = Robot;
                map[currentRow][currentColumn] = Empty;
                return (currentRow, currentColumn - 1);
            }
            else if (map[currentRow][currentColumn - 1] == Box)
            {
                bool canMove = true;
                int lastColumn = currentColumn - 1;
                for (; lastColumn > 0; lastColumn--)
                {
                    var character = map[currentRow][lastColumn - 1];
                    if (character == Box)
                    {
                        continue;
                    }
                    else if (character == Empty)
                    {
                        break;
                    }
                    else
                    {
                        canMove = false;
                        break;
                    }
                }
                if (canMove)
                {
                    for (; lastColumn < currentColumn; lastColumn++)
                    {
                        map[currentRow][lastColumn - 1] = Box;
                        map[currentRow][lastColumn] = Empty;
                    }
                    map[currentRow][currentColumn - 1] = Robot;
                    map[currentRow][currentColumn] = Empty;
                    return (currentRow, currentColumn - 1);
                }
            }
            return (currentRow, currentColumn);
        }

        private (int currentRow, int currentColumn) MoveRight(List<List<char>> map, int currentRow, int currentColumn)
        {
            if (map[currentRow][currentColumn + 1] == Empty)
            {
                map[currentRow][currentColumn + 1] = Robot;
                map[currentRow][currentColumn] = Empty;
                return (currentRow, currentColumn + 1);
            }
            else if (map[currentRow][currentColumn + 1] == Box)
            {
                bool canMove = true;
                int lastColumn = currentColumn + 1;
                for (; lastColumn < map[0].Count; lastColumn++)
                {
                    var character = map[currentRow][lastColumn + 1];
                    if (character == Box)
                    {
                        continue;
                    }
                    else if (character == Empty)
                    {
                        break;
                    }
                    else
                    {
                        canMove = false;
                        break;
                    }
                }
                if (canMove)
                {
                    for (; lastColumn > currentColumn; lastColumn--)
                    {
                        map[currentRow][lastColumn + 1] = Box;
                        map[currentRow][lastColumn] = Empty;
                    }
                    map[currentRow][currentColumn + 1] = Robot;
                    map[currentRow][currentColumn] = Empty;
                    return (currentRow, currentColumn + 1);
                }
            }
            return (currentRow, currentColumn);
        }

        private (int currentRow, int currentColumn) MoveUp(List<List<char>> map, int currentRow, int currentColumn)
        {
            if (map[currentRow - 1][currentColumn] == Empty)
            {
                map[currentRow - 1][currentColumn] = Robot;
                map[currentRow][currentColumn] = Empty;
                return (currentRow - 1, currentColumn);
            }
            else if (map[currentRow - 1][currentColumn] == Box)
            {
                bool canMove = true;
                int lastRow = currentRow - 1;
                for (; lastRow > 0; lastRow--)
                {
                    var character = map[lastRow - 1][currentColumn];
                    if (character == Box)
                    {
                        continue;
                    }
                    else if (character == Empty)
                    {
                        break;
                    }
                    else
                    {
                        canMove = false;
                        break;
                    }
                }
                if (canMove)
                {
                    for (; lastRow < currentRow; lastRow++)
                    {
                        map[lastRow - 1][currentColumn] = Box;
                        map[lastRow][currentColumn] = Empty;
                    }
                    map[currentRow - 1][currentColumn] = Robot;
                    map[currentRow][currentColumn] = Empty;
                    return (currentRow - 1, currentColumn);
                }
            }
            return (currentRow, currentColumn);
        }

        private (int currentRow, int currentColumn) MoveDown(List<List<char>> map, int currentRow, int currentColumn)
        {
            if (map[currentRow + 1][currentColumn] == Empty)
            {
                map[currentRow + 1][currentColumn] = Robot;
                map[currentRow][currentColumn] = Empty;
                return (currentRow + 1, currentColumn);
            }
            else if (map[currentRow + 1][currentColumn] == Box)
            {
                bool canMove = true;
                int lastRow = currentRow + 1;
                for (; lastRow < map.Count; lastRow++)
                {
                    var character = map[lastRow + 1][currentColumn];
                    if (character == Box)
                    {
                        continue;
                    }
                    else if (character == Empty)
                    {
                        break;
                    }
                    else
                    {
                        canMove = false;
                        break;
                    }
                }
                if (canMove)
                {
                    for (; lastRow > currentRow; lastRow--)
                    {
                        map[lastRow + 1][currentColumn] = Box;
                        map[lastRow][currentColumn] = Empty;
                    }
                    map[currentRow + 1][currentColumn] = Robot;
                    map[currentRow][currentColumn] = Empty;
                    return (currentRow + 1, currentColumn);
                }
            }
            return (currentRow, currentColumn);
        }

        private (List<List<char>> map, List<char> commands) ParseData(string[] data)
        {
            List<List<char>> map = new List<List<char>>();
            List<char> commands = new List<char>();
            bool generatingMap = true;
            foreach (var line in data)
            {
                if (generatingMap)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        generatingMap = false;
                        continue;
                    }
                    else
                    {
                        map.Add(line.ToList());
                    }
                }
                else
                {
                    commands.AddRange(line.ToList());
                }
            }
            return (map, commands);
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }
    }
}
