using System.Collections.Generic;
using System.Xml.Linq;
using static AdventOfCode2024.Day16;

namespace AdventOfCode2024
{
    public class Day18 : IDay
    {
        private readonly string _fileDayName = "Eighteen";
        public string GetName() => "Day 18";
        private const char Safe = '.';
        private const char Corrupted = '#';

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<KeyValuePair<int, int>> incomingBytes = new List<KeyValuePair<int, int>>();
            foreach (var line in data)
            {
                var lineData = line.Split(',').Select(s => int.Parse(s)).ToList();
                incomingBytes.Add(new KeyValuePair<int, int>(lineData[0], lineData[1]));
            }
            int mapLength = 71;
            int mapHeight = 71;
            int cycles = 1024;
            List<Node> map = GenerateMap(mapLength, mapHeight);
            for(var i = 0; i < cycles; i++)
            {
                var node = map.First(m => m.Row == incomingBytes[i].Key && m.Column == incomingBytes[i].Value);
                node.Character = Corrupted;
            }
            SetNeighbors(map);
            Node startingNode = map.First(m => m.Row == 0 && m.Column == 0);
            Node endNode = map.First(m => m.Row == mapHeight-1 && m.Column == mapLength-1);
            PerformDijkstras(startingNode, endNode);
            return endNode.DistanceFromStart;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        private static List<Node> GenerateMap(int length, int height)
        {
            List<Node> map = new();
            for (var row = 0; row < height; row++)
            {
                for(var column = 0; column < length; column++)
                {
                    map.Add(new Node(Safe, row, column));
                }
            }
            return map;
        }

        private void SetNeighbors(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var neighbor = nodes.FirstOrDefault(n => n.Row - 1 == node.Row && n.Column == node.Column);
                if (neighbor is not null && neighbor.Character != Corrupted)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row + 1 == node.Row && n.Column == node.Column);
                if (neighbor is not null && neighbor.Character != Corrupted)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row == node.Row && n.Column - 1 == node.Column);
                if (neighbor is not null && neighbor.Character != Corrupted)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row == node.Row && n.Column + 1 == node.Column);
                if (neighbor is not null && neighbor.Character != Corrupted)
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
