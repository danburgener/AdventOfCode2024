using AdventOfCode2024;

IDay dayToRun = new Day07();

Console.WriteLine(dayToRun.GetName());
Console.WriteLine($"One: {await dayToRun.One()}");
Console.WriteLine($"Two: {await dayToRun.Two()}");