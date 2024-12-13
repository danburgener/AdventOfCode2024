namespace AdventOfCode2024
{
    public class Day13 : IDay
    {
        private readonly string _fileDayName = "Thirteen";
        public string GetName() => "Day 13";
        private const int ACost = 3;
        private const int BCost = 1;

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            List<Game> games = GetGames(data);
            long count = 0;
            foreach (var game in games)
            {
                //Linear Algebra - Linear combinations
                //a<AX,AY> + b<BX,BY> = <PX,PY> ==> a<94,34> + b<22,67> = <8400,5400>
                //

                //AXa + BXb = PX ==> 94a + 22b = 8400

                //AYa + BYb = PY ==> 34a + 67b = 5400

                //AY(AXa + BXb) = AY(PX) ==> 34(94a + 22b) = 34(8400)
                //AX(AYa + BYb) = AX(PY) ==> 94(34a + 67b) = 94(5400)

                //(AYPX - AXPY) / (AYBXb - AXBYb)

                var AYBXb = game.ButtonA.Y * game.ButtonB.X;
                var AXBYb = game.ButtonA.X * game.ButtonB.Y;
                var AYPX = game.ButtonA.Y * game.Prize.X;
                var AXPY = game.ButtonA.X * game.Prize.Y;

                var b = (AYPX - AXPY) / (AYBXb - AXBYb);

                var a = (game.Prize.X - (game.ButtonB.X * b)) / game.ButtonA.X;

                if (a % 1 == 0 && b % 1 == 0)
                {
                    count += ((long)a * ACost) + ((long)b * BCost);
                }

            }
            return count;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            List<Game> games = GetGames(data, 10000000000000d);
            long count = 0;
            foreach (var game in games)
            {
                var AYBXb = game.ButtonA.Y * game.ButtonB.X;
                var AXBYb = game.ButtonA.X * game.ButtonB.Y;
                var AYPX = game.ButtonA.Y * game.Prize.X;
                var AXPY = game.ButtonA.X * game.Prize.Y;

                var b = (AYPX - AXPY) / (AYBXb - AXBYb);

                var a = (game.Prize.X - (game.ButtonB.X * b)) / game.ButtonA.X;

                if (a % 1 == 0 && b % 1 == 0)
                {
                    count += ((long)a * ACost) + ((long)b * BCost);
                }

            }
            return count;
        }

        private List<Game> GetGames(string[] data, double amountToAddToPrize = 0.0d)
        {
            List<Game> games = [];
            for (var i = 0; i < data.Length; i++)
            {
                var buttonALine = data[i++];
                var buttonBLine = data[i++];
                var prizeLine = data[i++];
                games.Add(new Game()
                {
                    ButtonA = GetButton(buttonALine),
                    ButtonB = GetButton(buttonBLine),
                    Prize = GetPrize(prizeLine, amountToAddToPrize)
                });
            }
            return games;
        }

        private DoubleVector2 GetPrize(string prizeLine, double amountToAddToPrize = 0.0d)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"Prize:\sX=(\d+),\sY=(\d+)");
            var match = regex.Match(prizeLine);
            return new DoubleVector2(double.Parse(match.Groups[1].Value) + amountToAddToPrize, double.Parse(match.Groups[2].Value) + amountToAddToPrize);
        }

        private DoubleVector2 GetButton(string buttonLine)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"Button\s[AB]\:\sX\+(\d+),\sY\+(\d+)");
            var match = regex.Match(buttonLine);
            return new DoubleVector2(double.Parse(match.Groups[1].Value), double.Parse(match.Groups[2].Value));
        }

        public class Game
        {
            public DoubleVector2 ButtonA { get; set; }
            public DoubleVector2 ButtonB { get; set; }
            public DoubleVector2 Prize { get; set; }
        }

        public class DoubleVector2
        {
            public DoubleVector2(double x, double y)
            {
                X = x;
                Y = y;
            }
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
