using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2024
{
    public class Day20 : IDay
    {
        private readonly string _fileDayName = "Twenty";
        public string GetName() => "Day 20";
        private const char Track = '.';
        private const char Wall = '#';
        private const char Start = 'S';
        private const char End = 'E';

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            var map = GenerateMap(data);
            SetNeighbors(map);
            Node startingNode = map.First(m => m.Character == Start);
            Node endNode = map.First(m => m.Character == End);
            long fastestWithoutCheating = PerformDijkstras(startingNode, endNode);
            return Cheat(map, fastestWithoutCheating);
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
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

        private void SetNeighbors(List<Node> nodes)
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
                    if (neighbor.Character == Wall || visited.Contains(neighbor))
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

        private long Cheat(List<Node>? originalMap, long fastestWithoutCheating)
        {
            return 0;
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
