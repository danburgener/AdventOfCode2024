


namespace AdventOfCode2024
{
    public class Day12 : IDay
    {
        private readonly string _fileDayName = "Twelve";
        public string GetName() => "Day 12";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<List<GardenPlot>> gardenMap = GenerateMap(data);
            List<GardenPlot> visited = new List<GardenPlot>();
            long costOfRegions = 0;
            foreach(var row in gardenMap)
            {
                foreach(var gardenPlot in row)
                {
                    if (visited.Contains(gardenPlot))
                    {
                        continue;
                    }
                    costOfRegions += CalculateCostOfRegion(gardenPlot, visited);
                }
            }
            return costOfRegions;
        }

        private long CalculateCostOfRegion(GardenPlot gardenPlot, List<GardenPlot> visited)
        {
            var regionData = CalculateRegion(gardenPlot, visited);
            return regionData.fences * regionData.regionSize;
        }

        private (int fences, int regionSize) CalculateRegion(GardenPlot gardenPlot, List<GardenPlot> visited)
        {
            if (visited.Contains(gardenPlot))
            {
                return (0, 0);
            }
            int fences = 0;
            int regionSize = 1;
            visited.Add(gardenPlot);
            List<GardenPlot> neighborsToVisit = new List<GardenPlot>();
            foreach (var neighbor in gardenPlot.Neighbors)
            {
                if (neighbor.TypeChar != gardenPlot.TypeChar)
                {
                    fences++;
                    continue;
                }
                else if (visited.Contains(neighbor))
                {
                    continue;
                }
                else
                {
                    neighborsToVisit.Add(neighbor);
                }
            }
            foreach(var neighborToVisit in neighborsToVisit)
            {
                var regionData = CalculateRegion(neighborToVisit, visited);
                fences += regionData.fences;
                regionSize += regionData.regionSize;
            }
            return (fences, regionSize);
        }

        private List<List<GardenPlot>> GenerateMap(string[] data)
        {
            List<List<GardenPlot>> map = new List<List<GardenPlot>>();
            foreach(var row in data)
            {
                List<GardenPlot> gardenRow = new List<GardenPlot>();
                foreach(var column in row)
                {
                    gardenRow.Add(new GardenPlot(column));
                }
                map.Add(gardenRow);
            }
            SetNeighbors(map);
            return map;
        }

        private void SetNeighbors(List<List<GardenPlot>> map)
        {
            for(int row = 0; row < map.Count(); row++)
            {
                for (int column = 0; column < map[0].Count(); column++)
                {
                    var currentGardenPlot = map[row][column];
                    //TOP
                    if (row > 0)
                    {
                        currentGardenPlot.AddNeighbor(map[row - 1][column]);
                    }
                    else
                    {
                        currentGardenPlot.AddNeighbor(new GardenPlot(null));
                    }

                    //RIGHT
                    if (column < map[0].Count - 1)
                    {
                        currentGardenPlot.AddNeighbor(map[row][column+1]);
                    }
                    else
                    {
                        currentGardenPlot.AddNeighbor(new GardenPlot(null));
                    }

                    //BOTTOM
                    if (row < map.Count-1)
                    {
                        currentGardenPlot.AddNeighbor(map[row + 1][column]);
                    }
                    else
                    {
                        currentGardenPlot.AddNeighbor(new GardenPlot(null));
                    }

                    //LEFT
                    if (column > 0)
                    {
                        currentGardenPlot.AddNeighbor(map[row][column-1]);
                    }
                    else
                    {
                        currentGardenPlot.AddNeighbor(new GardenPlot(null));
                    }
                }
            }
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        public class GardenPlot
        {
            public GardenPlot(char? typeChar)
            {
                TypeChar = typeChar;
            }
            public char? TypeChar { get; private set; }
            public List<GardenPlot> Neighbors { get; private set; } = new List<GardenPlot>();

            public void AddNeighbor(GardenPlot neighbor)
            {
                Neighbors.Add(neighbor);
            }
        }
    }
}
