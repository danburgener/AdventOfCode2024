﻿using AdventOfCode2024;

IDay dayToRun = new Day16();

Console.WriteLine(dayToRun.GetName());
Console.WriteLine($"One: {await dayToRun.One()}");
Console.WriteLine($"Two: {await dayToRun.Two()}");