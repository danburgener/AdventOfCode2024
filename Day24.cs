namespace AdventOfCode2024
{
    public class Day24 : IDay
    {
        private readonly string _fileDayName = "TwentyFour";
        public string GetName() => "Day 24";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            Dictionary<string, bool> wires = new Dictionary<string, bool>();
            Queue<Command> commands = new();
            bool gettingInitialWireValues = true;
            foreach(var line in data)
            {
                if (string.IsNullOrEmpty(line))
                {
                    gettingInitialWireValues = false;
                    continue;
                }
                if (gettingInitialWireValues)
                {
                    var splitLine = line.Split(':');
                    wires.Add(splitLine[0].Trim(), splitLine[1].Trim() == "1");
                }
                else
                {
                    var splitLine = line.Split(' ');
                    commands.Enqueue(new Command
                    {
                        InputWire1 = splitLine[0],
                        InputWire2 = splitLine[2],
                        Gate = splitLine[1],
                        OutputWire = splitLine[4]
                    });
                }
            }
            while (commands.Count > 0)
            {
                var command = commands.Dequeue();

                if (wires.ContainsKey(command.InputWire1) && wires.ContainsKey(command.InputWire2))
                {

                    bool value;
                    if (command.Gate == "AND")
                    {
                        value = wires[command.InputWire1] && wires[command.InputWire2];
                    }
                    else if (command.Gate == "XOR")
                    {
                        value = wires[command.InputWire1] != wires[command.InputWire2];
                    }
                    else if (command.Gate == "OR")
                    {
                        value = wires[command.InputWire1] || wires[command.InputWire2];
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    if (!wires.ContainsKey(command.OutputWire))
                    {
                        wires.Add(command.OutputWire, value);
                    }
                    else
                    {
                        wires[command.OutputWire] = value;
                    }
                }
                else
                {
                    commands.Enqueue(command);
                }

            }
            foreach(var command in commands)
            {
                
            }
            var zWires = wires.Where(w => w.Key.StartsWith("z")).OrderByDescending(w => w.Key);
            string binaryNumber = string.Empty;
            foreach(var zWire in zWires)
            {
                binaryNumber += zWire.Value ? "1" : "0";
            }
            return Convert.ToInt64(binaryNumber, 2);
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        private class Command
        {
            public string InputWire1 { get; set; }
            public string InputWire2 { get; set; }
            public string Gate { get; set; }
            public string OutputWire { get; set; }
        }
    }
}
