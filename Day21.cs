
namespace AdventOfCode2024
{
    public class Day21 : IDay
    {
        private readonly string _fileDayName = "TwentyOne";
        public string GetName() => "Day 21";
        private const char A = 'A';


        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            var directionalYouMap = GenerateDirectionalMap();
            var directionalRobot1Map = GenerateDirectionalMap();
            var directionalRobot2Map = GenerateDirectionalMap();
            var numbericKeypadRobotMap = GenerateNumbericKeypadMap();

            int totalValue = 0;
            foreach (var line in data)
            {
                var numbericValue = int.Parse(line.Substring(0, line.Length-1));
                List<char> commandsForNumbericKeypad = GetCommands(line.ToList(), numbericKeypadRobotMap);
                List<char> commandsForDirectionalRobot1 = GetCommands(commandsForNumbericKeypad, directionalRobot1Map);
                List<char> commandsForDirectionalRobot2 = GetCommands(commandsForDirectionalRobot1, directionalRobot2Map);
                totalValue += numbericValue * commandsForDirectionalRobot2.Count();
            }

            return totalValue;
        }

        private List<char> GetCommands(List<char> line, List<Node> map)
        {
            char currentLocation = A;
            var itemsToSelect = line;
            List<char> commands = new List<char>();
            foreach (var item in itemsToSelect)
            {
                List<char> directions = new List<char>();
                if (currentLocation != item)
                {
                    Node startingNode = map.First(n => n.Character == currentLocation);
                    Node endNode = map.First(n => n.Character == item);

                    PerformDijkstras(startingNode, endNode);
                    Node previous = endNode.Previous;
                    Node currentNode = endNode;
                    do
                    {
                        directions.Insert(0, GetDirection(previous, currentNode));
                        previous = previous.Previous;
                        currentNode = currentNode.Previous;
                    } while (previous != null);
                }
                directions.Add(A);
                commands.AddRange(directions);
                currentLocation = item;
                RestartMap(map);
            }

            return commands;
        }

        private List<char> GetCommandsForDirectionalRobot1(List<char> line, List<Node> directionalRobot1Map)
        {
            char currentLocation = A;
            var itemsToSelect = line;
            List<char> commandsForDirectionalKeypad = new List<char>();
            foreach (var item in itemsToSelect)
            {
                List<char> directions = new List<char>();
                if (currentLocation != item)
                {
                    Node startingNode = directionalRobot1Map.First(n => n.Character == currentLocation);
                    Node endNode = directionalRobot1Map.First(n => n.Character == item);

                    PerformDijkstras(startingNode, endNode);
                    Node previous = endNode.Previous;
                    Node currentNode = endNode;
                    do
                    {
                        directions.Insert(0, GetDirection(previous, currentNode));
                        previous = previous.Previous;
                        currentNode = currentNode.Previous;
                    } while (previous != null);
                }
                directions.Add(A);
                commandsForDirectionalKeypad.AddRange(directions);
                currentLocation = item;
                RestartMap(directionalRobot1Map);
            }

            return commandsForDirectionalKeypad;
        }

        private void RestartMap(List<Node> numbericKeypadRobotMap)
        {
            numbericKeypadRobotMap.ForEach(n =>
            {
                n.Previous = null;
                n.DistanceFromStart = long.MaxValue;
            });
        }

        private char GetDirection(Node start, Node end)
        {
            if (start.Row > end.Row)
            {
                return '^';
            }
            else if (start.Row < end.Row)
            {
                return 'v';
            }
            else if (start.Column > end.Column)
            {
                return '<';
            }
            else
            {
                return '>';
            }
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        private static List<Node> GenerateDirectionalMap()
        {
            string[] data = [
                " ^A",
                "<v>"
            ];
            List<Node> map = new();
            for (var row = 0; row < data.Length; row++)
            {
                for (var column = 0; column < data[0].Length; column++)
                {
                    map.Add(new Node(data[row][column], row, column));
                }
            }
            SetNeighbors(map);
            return map;
        }

        private static List<Node> GenerateNumbericKeypadMap()
        {
            string[] data = [
                "789",
                "456",
                "123",
                " 0A"
            ];
            List<Node> map = new();
            for (var row = 0; row < data.Length; row++)
            {
                for (var column = 0; column < data[0].Length; column++)
                {
                    map.Add(new Node(data[row][column], row, column));
                }
            }
            SetNeighbors(map);
            return map;
        }

        private static List<Node> GenerateMap(string[] data)
        {
            List<Node> map = new();
            for (var row = 0; row < data.Length; row++)
            {
                for (var column = 0; column < data[0].Length; column++)
                {
                    map.Add(new Node(data[row][column], row, column));
                }
            }
            return map;
        }

        private static void SetNeighbors(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var neighbor = nodes.FirstOrDefault(n => n.Row - 1 == node.Row && n.Column == node.Column);
                if (neighbor is not null)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row + 1 == node.Row && n.Column == node.Column);
                if (neighbor is not null)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row == node.Row && n.Column - 1 == node.Column);
                if (neighbor is not null)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row == node.Row && n.Column + 1 == node.Column);
                if (neighbor is not null)
                {
                    node.Neighors.Add(neighbor);
                }
            }
        }

        private long PerformDijkstras(Node startingNode, Node endNode)
        {
            List<Node> unvisited = new List<Node>() { startingNode };
            List<Node> visited = new List<Node>();

            startingNode.DistanceFromStart = 0;
            Node? currentNode = startingNode;
            while (currentNode is not null)
            {
                unvisited.Remove(currentNode);
                visited.Add(currentNode);

                foreach (var neighbor in currentNode.Neighors)
                {
                    if (visited.Contains(neighbor))
                    {
                        continue;
                    }
                    if (!unvisited.Contains(neighbor))
                    {
                        unvisited.Add(neighbor);
                    }
                    long newDistance = currentNode.DistanceFromStart + 1;
                    if (neighbor.DistanceFromStart > newDistance)
                    {
                        neighbor.DistanceFromStart = newDistance;
                        neighbor.Previous = currentNode;
                    }
                }

                currentNode = unvisited.OrderBy(x => x.DistanceFromStart).FirstOrDefault();
            }

            return endNode.DistanceFromStart;

        }

        public class Node
        {
            public Node(char character, int row, int column)
            {
                Character = character;
                Row = row;
                Column = column;
                Neighors = new List<Node>();
            }

            public char Character { get; set; }
            public int Row { get; }
            public int Column { get; }
            public List<Node> Neighors { get; }
            public long DistanceFromStart { get; set; } = long.MaxValue;
            public Node? Previous { get; set; }
        }
    }
}
