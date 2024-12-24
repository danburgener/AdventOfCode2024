

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
                List<char> commandsForNumbericKeypad = GetCommandsBFS(line.ToList(), numbericKeypadRobotMap);
                //List<char> commandsForNumbericKeypad = GetCommands(line.ToList(), numbericKeypadRobotMap);
                List<char> commandsForDirectionalRobot1 = GetCommandsBFS(commandsForNumbericKeypad, directionalRobot1Map);
                List<char> commandsForDirectionalRobot2 = GetCommandsBFS(commandsForDirectionalRobot1, directionalRobot2Map);
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

        private List<char> GetCommandsBFS(List<char> line, List<Node> map)
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
                    var paths = BFS(startingNode, endNode);
                    List<Node> straightestPath = new();
                    if (paths.Count == 1)
                    {
                        straightestPath = paths[0];
                    }
                    else
                    {
                        int leastAmountOfChanges = int.MaxValue;
                        foreach (var path in paths)
                        {
                            int changeCount = 0;
                            for (var i = 0; i < path.Count - 1; i++)
                            {
                                if (path[i].Character != path[i + 1].Character)
                                {
                                    changeCount++;
                                }
                            }
                            if (leastAmountOfChanges > changeCount)
                            {
                                straightestPath = path;
                            }
                        }
                    }
                    
                    for(var i = 1; i < straightestPath.Count; i++)
                    {
                        directions.Insert(0, GetDirection(straightestPath[i-1], straightestPath[i]));
                    }
                }
                directions.Add(A);
                commands.AddRange(directions);
                currentLocation = item;
            }

            return commands;
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
                    if (visited.Contains(neighbor) || neighbor.Character == 32)
                    {
                        continue;
                    }
                    if (!unvisited.Contains(neighbor))
                    {
                        unvisited.Add(neighbor);
                    }
                    long newDistance = currentNode.DistanceFromStart + 1;
                    if (neighbor.DistanceFromStart >= newDistance)
                    {
                        neighbor.DistanceFromStart = newDistance;
                        neighbor.Previous = currentNode;
                    }
                }

                currentNode = unvisited.OrderBy(x => x.DistanceFromStart).FirstOrDefault();
            }

            return endNode.DistanceFromStart;

        }

        private List<List<Node>> BFS(Node startingNode, Node endNode)
        {
            Queue<List<Node>> queue = new();

            List<Node> path = [startingNode];
            queue.Enqueue(path);
            List<Node> visited = [];

            List<List<Node>> completePaths = new();
            int shortestPath = int.MaxValue;

            while (queue.Count != 0)
            {
                path = queue.Dequeue();
                if (path.Count > shortestPath)
                {
                    continue;
                }
                Node last = path.Last();
                if (last == endNode)
                {
                    shortestPath = path.Count;
                    completePaths.Add(path);
                }

                foreach(var neighbor in last.Neighors)
                {
                    if (!path.Contains(neighbor))
                    {
                        List<Node> newPath = new List<Node>(path)
                        {
                            neighbor
                        };
                        queue.Enqueue(newPath);
                    }
                }
            }
            return completePaths;
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
