namespace AdventOfCode2024
{
    public class Day10 : IDay
    {
        private readonly string _fileDayName = "Ten";
        public string GetName() => "Day 10";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<Node> allNodes = GetAllNodes(data);
            List<Node> trailheads = allNodes.Where(n => n.Height == 0).ToList();
            int count = 0;
            foreach (var trailhead in trailheads)
            {
                count += GetTrailheadPathsDistinct(trailhead, allNodes);
            }
            return count;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            List<Node> allNodes = GetAllNodes(data);
            List<Node> trailheads = allNodes.Where(n => n.Height == 0).ToList();
            int count = 0;
            foreach (var trailhead in trailheads)
            {
                count += GetTrailheadPaths(trailhead, allNodes);
            }
            return count;
        }

        private int GetTrailheadPathsDistinct(Node trailhead, List<Node> allNodes)
        {
            var reachableTops = IsPath(trailhead, allNodes);
            return reachableTops.Distinct().Count();
        }

        private int GetTrailheadPaths(Node trailhead, List<Node> allNodes)
        {
            return IsPath(trailhead, allNodes).Count();
        }

        private List<Node> IsPath(Node currentLocation, List<Node> allNodes)
        {
            List<Node> reachableTops = new List<Node>();
            if (currentLocation.Height == 9)
            {
                reachableTops.Add(currentLocation);
                return reachableTops;
            }
            else
            {
                List<Node> higherNeighbors = GetHigherNeighbors(currentLocation, allNodes);
                foreach (var higherNeighbor in higherNeighbors)
                {
                    reachableTops.AddRange(IsPath(higherNeighbor, allNodes));
                }
                return reachableTops;
            }
        }

        private List<Node> GetHigherNeighbors(Node currentLocation, List<Node> allNodes)
        {
            List<Node> higherNeighbors = new();
            int row = currentLocation.Row;
            int column = currentLocation.Column;
            var nextHighest = allNodes.Where(a => a.Height == currentLocation.Height + 1);
            var above = nextHighest.FirstOrDefault(nh => nh.Row == row - 1 && nh.Column == column);
            var below = nextHighest.FirstOrDefault(nh => nh.Row == row + 1 && nh.Column == column);
            var left = nextHighest.FirstOrDefault(nh => nh.Row == row && nh.Column == column - 1);
            var right = nextHighest.FirstOrDefault(nh => nh.Row == row && nh.Column == column + 1);
            if (above is not null)
            {
                higherNeighbors.Add(above);
            }
            if (below is not null)
            {
                higherNeighbors.Add(below);
            }
            if (left is not null)
            {
                higherNeighbors.Add(left);
            }
            if (right is not null)
            {
                higherNeighbors.Add(right);
            }
            return higherNeighbors;
        }

        private List<Node> GetAllNodes(string[] data)
        {
            List<Node> nodes = new List<Node>();
            for (var row = 0; row < data.Length; row++)
            {
                for (var column = 0; column < data[0].Length; column++)
                {
                    nodes.Add(new Node
                    {
                        Row = row,
                        Column = column,
                        Height = int.Parse(data[row][column].ToString())
                    });
                }
            }
            return nodes;
        }

        public class Node
        {
            public int Height { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
        }
    }
}
