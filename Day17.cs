namespace AdventOfCode2024
{
    public class Day17 : IDay
    {
        private readonly string _fileDayName = "Seventeen";
        public string GetName() => "Day 17";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            long registerA = long.Parse(data[0].Split(':')[1]);
            long registerB = long.Parse(data[1].Split(':')[1]);
            long registerC = long.Parse(data[2].Split(':')[1]);

            List<int> program = data[4].Split(':')[1].Trim().Split(',').Select(s => int.Parse(s)).ToList();

            Console.WriteLine(ProcessData(registerA, registerB, registerC, program));
            return 0;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            long registerA = 0; //This is corrupt, so we will figure it out later
            long registerB = long.Parse(data[1].Split(':')[1]);
            long registerC = long.Parse(data[2].Split(':')[1]);

            string originalProgram = data[4].Split(':')[1].Trim();
            List<string> originalProgramSplit = originalProgram.Split(',').ToList();
            string finalOutput = string.Empty;
            List<int> program = originalProgramSplit.Select(s => int.Parse(s)).ToList();
            int programLength = program.Count();
            int cyclesToChangeNextNumber = 8; //The first digit changes 8 times before the second digit changes. 8^n
            int currentPower = programLength - 1;
            registerA = (long)Math.Pow(cyclesToChangeNextNumber, currentPower);
            while (true)
            {
                finalOutput = ProcessData(registerA, registerB, registerC, program);
                if (finalOutput == originalProgram)
                {
                    break;
                }
                else
                {
                    var finalOutputSplit = finalOutput.Split(',');
                    for (int i = programLength-1; i >= 0; i--)
                    {
                        if (finalOutputSplit[i] == originalProgramSplit[i])
                        {
                            continue;
                        }
                        else
                        {
                            currentPower = i;
                            break;
                        }
                    }
                    registerA += (long)Math.Pow(cyclesToChangeNextNumber, currentPower);
                }
            };
            return registerA;
        }

        private string ProcessData(long registerA, long registerB, long registerC, List<int> program)
        {
            string finalOutput = string.Empty;
            int pointerIncreaseAmount = 2;
            for (int pointer = 0; pointer < program.Count; pointer += pointerIncreaseAmount)
            {
                pointerIncreaseAmount = 2;
                switch (program[pointer])
                {
                    case 0:
                        ProcessAdv(ref registerA, program[pointer + 1], registerB, registerC); break;
                    case 1:
                        ProcessBxl(ref registerB, program[pointer + 1]); break;
                    case 2:
                        ProcessBst(ref registerB, program[pointer + 1], registerA, registerC); break;
                    case 3:
                        var result = ProcessJnz(registerA, program[pointer + 1], ref pointer);
                        if (result)
                        {
                            pointerIncreaseAmount = 0;
                        }
                        break;
                    case 4:
                        ProcessBxc(ref registerB, registerC); break;
                    case 5:
                        var output = ProcessOut(program[pointer + 1], registerA, registerB, registerC);
                        if (string.IsNullOrEmpty(finalOutput))
                        {
                            finalOutput = output;
                        }
                        else
                        {
                            finalOutput = $"{finalOutput},{output}";
                        }
                        break;
                    case 6:
                        ProcessBdv(registerA, ref registerB, program[pointer + 1], registerC); break;
                    case 7:
                        ProcessCdv(registerA, ref registerC, program[pointer + 1], registerB); break;
                }
            }
            return finalOutput;
        }

        private long GetComboOperand(int operand, long registerA, long registerB, long registerC)
        {
            if (operand >= 0 && operand <= 3)
            {
                return operand;
            }else if (operand == 4)
            {
                return registerA;
            }
            else if (operand == 5)
            {
                return registerB;
            }
            else if (operand == 6)
            {
                return registerC;
            }
            else
            {
                throw new Exception();
            }
        }

        private void ProcessAdv(ref long registerA, int operand, long registerB, long registerC)
        {
            registerA = (long)(registerA / (Math.Pow(2, GetComboOperand(operand, registerA, registerB, registerC))));
        }
        private void ProcessBxl(ref long registerB, int operand)
        {
            registerB = registerB ^ operand;
        }

        private void ProcessBst(ref long registerB, int operand, long registerA, long registerC)
        {
            registerB = GetComboOperand(operand, registerA, registerB, registerC) % 8;
        }

        //A return of true means the pointer shouldn't move forward after
        private bool ProcessJnz(long registerA, int operand, ref int currentPointerIndex)
        {
            if (registerA == 0)
            {
                return false;
            }
            else
            {
                currentPointerIndex = operand;
                return true;
            }
        }

        private void ProcessBxc(ref long registerB, long registerC)
        {
            registerB = registerB ^ registerC;
        }

        private string ProcessOut(int operand, long registerA, long registerB, long registerC)
        {
            var output = GetComboOperand(operand, registerA, registerB, registerC) % 8;
            return output.ToString();
        }

        private void ProcessBdv(long registerA, ref long registerB, int operand, long registerC)
        {
            registerB = (long)(registerA / (Math.Pow(2, GetComboOperand(operand, registerA, registerB, registerC))));
        }

        private void ProcessCdv(long registerA, ref long registerC, int operand, long registerB)
        {
            registerC = (long)(registerA / (Math.Pow(2, GetComboOperand(operand, registerA, registerB, registerC))));
        }
    }
}
