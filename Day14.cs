
namespace AdventOfCode2024
{
    public class Day14 : IDay
    {
        private readonly string _fileDayName = "Fourteen";
        public string GetName() => "Day 14";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<Robot> robots = GenerateRobots(data);
            int tilesWide = 101;
            int tilesTall = 103;
            int secondsToElapse = 100;
            return GetSafetyFactor(robots, tilesWide, tilesTall, secondsToElapse);
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            List<Robot> robots = GenerateRobots(data);
            int tilesWide = 101;
            int tilesTall = 103;
            int secondsElapsed = 0;
            while(true && secondsElapsed < int.MaxValue)
            {
                secondsElapsed++;
                foreach (var robot in robots)
                {
                    MoveRobot(robot, tilesWide, tilesTall);
                }
                if (HasStraightLine(robots, tilesTall, 10))
                {
                    PrintRobots(robots, tilesWide, tilesTall);
                    return secondsElapsed;
                }
            }
            return 0;
        }

        private static bool HasStraightLine(List<Robot> robots, int tilesTall, int minLineCount)
        {
            for(int row = 0; row < tilesTall; row++)
            {
                var robotsOnRow = robots.Where(r => r.PositionY == row).OrderBy(r => r.PositionX);
                if (robotsOnRow.Count() >= minLineCount)
                {
                    bool inStraightLine = true;
                    for(var i = 0; i < robotsOnRow.Count()-1; i++)
                    {
                        if (robotsOnRow.ElementAt(i).PositionX != robotsOnRow.ElementAt(i + 1).PositionX - 1)
                        {
                            inStraightLine = false;
                            break;
                        }
                    }
                    return inStraightLine;
                }
            }
            return false;
        }

        private long GetSafetyFactor(List<Robot> robots, int tilesWide, int tilesTall, int secondsToElapse)
        {
            for (int i = 0; i < secondsToElapse; i++)
            {
                foreach (var robot in robots)
                {
                    MoveRobot(robot, tilesWide, tilesTall);
                }
                
            }

            return GetSafetyFactor(robots, tilesWide, tilesTall);
        }

        private void PrintRobots(List<Robot> robots, int tilesWide, int tilesTall)
        {
            Console.Clear();
            var map = Enumerable.Range(0, tilesTall)
                      .Select(x => Enumerable.Repeat(0, tilesWide).ToList())
                      .ToList();
            foreach(var robot in robots)
            {
                map[robot.PositionY][robot.PositionX]++;
            }
            foreach(var line in map)
            {
                Console.WriteLine(string.Join("", line));
            }
        }

        private long GetSafetyFactor(List<Robot> robots, int tilesWide, int tilesTall)
        {
            int middleX = tilesWide / 2;
            int middleY = tilesTall / 2;
            var removedCount = robots.RemoveAll(r => r.PositionX == middleX || r.PositionY == middleY);
            long safetyFactor = 1;
            safetyFactor *= GetSafetyFactor(robots.Where(r => r.PositionX < middleX && r.PositionY < middleY).ToList()); //TOP LEFT
            safetyFactor *= GetSafetyFactor(robots.Where(r => r.PositionX > middleX && r.PositionY < middleY).ToList()); //TOP RIGHT
            safetyFactor *= GetSafetyFactor(robots.Where(r => r.PositionX > middleX && r.PositionY > middleY).ToList()); //BOTTOM RIGHT
            safetyFactor *= GetSafetyFactor(robots.Where(r => r.PositionX < middleX && r.PositionY > middleY).ToList()); //BOTTOM LEFT
            return safetyFactor;
        }

        private long GetSafetyFactor(List<Robot> robots)
        {
            var groups = robots.GroupBy(r => $"{r.PositionX}-{r.PositionY}");
            var safetyFactor = 0;
            foreach(var group in groups)
            {
                safetyFactor += group.Count();
            }
            return safetyFactor;
        }

        private void MoveRobot(Robot robot, int tilesWide, int tilesTall)
        {
            robot.PositionX = robot.PositionX + robot.VelocityX;
            if (robot.PositionX < 0)
            {
                robot.PositionX = tilesWide + robot.PositionX;
            }
            else if (robot.PositionX >= tilesWide)
            {
                robot.PositionX = robot.PositionX - tilesWide;
            }
            robot.PositionY = robot.PositionY + robot.VelocityY;
            if (robot.PositionY < 0)
            {
                robot.PositionY = tilesTall + robot.PositionY;
            }
            else if (robot.PositionY >= tilesTall)
            {
                robot.PositionY = robot.PositionY - tilesTall;
            }
        }

        private List<Robot> GenerateRobots(string[] data)
        {
            List<Robot> robots = [];
            var regex = new System.Text.RegularExpressions.Regex(@"p=(-*\d+),(-*\d+)\sv=(-*\d+),(-*\d+)");
            foreach (var line in data)
            {
                var match = regex.Match(line);
                robots.Add(new Robot(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
            }
            return robots;
        }

        public class Robot
        {
            public Robot(int positionX, int positionY, int velocityX, int velocityY)
            {
                PositionX = positionX;
                PositionY = positionY;
                VelocityX = velocityX;
                VelocityY = velocityY;
            }

            public int PositionX { get; set; }
            public int PositionY { get; set; }
            public int VelocityX { get; set; }
            public int VelocityY { get; set; }
        }
    }
}
