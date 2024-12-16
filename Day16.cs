
namespace AdventOfCode2024
{
    public class Day16 : IDay
    {
        private readonly string _fileDayName = "Sixteen";
        public string GetName() => "Day 16";
        private const char Start = 'S';
        private const char End = 'E';
        private const char Wall = '#';
        private const int TurnCost = 1000;

        public enum Direction
        {
            North,
            East,
            South,
            West
        }

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<Node> nodes = new List<Node>();
            for (var row = 0; row < data.Length; row++)
            {
                for (var column = 0; column < data[0].Length; column++)
                {
                    nodes.Add(new Node(data[row][column], row, column));
                }
            }
            SetNeighbors(nodes);
            var startingNode = nodes.First(n => n.Character == Start);
            startingNode.Direction = Direction.East;
            var endNode = nodes.First(n => n.Character == End);
            return PerformDijkstras(startingNode, endNode);
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
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
                    long newDistance = GetDistance(currentNode, neighbor);
                    if (neighbor.DistanceFromStart > newDistance)
                    {
                        neighbor.Direction = GetNewDirection(currentNode, neighbor);
                        neighbor.DistanceFromStart = newDistance;
                        neighbor.Previous = currentNode;
                    }
                }

                currentNode = unvisited.OrderBy(x => x.DistanceFromStart).FirstOrDefault();
            }

            return endNode.DistanceFromStart;

        }

        private Direction GetNewDirection(Node currentNode, Node neighbor)
        {
            //up
            if (currentNode.Column == neighbor.Column && currentNode.Row > neighbor.Row)
            {
                return Direction.North;
            }
            //right
            else if (currentNode.Column < neighbor.Column && currentNode.Row == neighbor.Row)
            {
                return Direction.East;
            }
            //down
            else if (currentNode.Column == neighbor.Column && currentNode.Row < neighbor.Row)
            {
                return Direction.South;
            }
            //left
            else if (currentNode.Column > neighbor.Column && currentNode.Row == neighbor.Row)
            {
                return Direction.West;
            }
            else
            {
                throw new Exception();
            }
        }

        private long GetDistance(Node currentNode, Node neighbor)
        {
            int cost = 0;
            //up
            if (currentNode.Column == neighbor.Column && currentNode.Row > neighbor.Row)
            {
                if (currentNode.Direction == Direction.North)
                {
                    cost = 1;
                }else if (currentNode.Direction == Direction.South)
                {
                    cost = (TurnCost*2) + 1;
                }
                else
                {
                    cost = TurnCost + 1;
                }
            }
            //right
            else if (currentNode.Column < neighbor.Column && currentNode.Row == neighbor.Row)
            {
                if (currentNode.Direction == Direction.East)
                {
                    cost = 1;
                }
                else if (currentNode.Direction == Direction.West)
                {
                    cost = (TurnCost * 2) + 1;
                }
                else
                {
                    cost = TurnCost + 1;
                }
            }
            //down
            else if (currentNode.Column == neighbor.Column && currentNode.Row < neighbor.Row)
            {
                if (currentNode.Direction == Direction.South)
                {
                    cost = 1;
                }
                else if (currentNode.Direction == Direction.North)
                {
                    cost = (TurnCost * 2) + 1;
                }
                else
                {
                    cost = TurnCost + 1;
                }
            }
            //left
            else if (currentNode.Column > neighbor.Column && currentNode.Row == neighbor.Row)
            {
                if (currentNode.Direction == Direction.West)
                {
                    cost = 1;
                }
                else if (currentNode.Direction == Direction.East)
                {
                    cost = (TurnCost * 2) + 1;
                }
                else
                {
                    cost = TurnCost + 1;
                }
            }
            else
            {
                throw new Exception();
            }
            return currentNode.DistanceFromStart + cost;
        }

        private void SetNeighbors(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var neighbor = nodes.FirstOrDefault(n => n.Row - 1 == node.Row && n.Column == node.Column);
                if (neighbor is not null && neighbor.Character != Wall)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row + 1 == node.Row && n.Column == node.Column);
                if (neighbor is not null && neighbor.Character != Wall)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row == node.Row && n.Column - 1 == node.Column);
                if (neighbor is not null && neighbor.Character != Wall)
                {
                    node.Neighors.Add(neighbor);
                }
                neighbor = nodes.FirstOrDefault(n => n.Row == node.Row && n.Column + 1 == node.Column);
                if (neighbor is not null && neighbor.Character != Wall)
                {
                    node.Neighors.Add(neighbor);
                }
            }
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

            public char Character { get; }
            public int Row { get; }
            public int Column { get; }
            public List<Node> Neighors { get; }
            public long DistanceFromStart { get; set; } = long.MaxValue;
            public Node? Previous { get; set; }
            public Direction Direction { get; set; }
        }
    }
}
