
using Microsoft.Win32;

namespace AdventOfCode2024
{
    public class Day17 : IDay
    {
        private readonly string _fileDayName = "Seventeen";
        public string GetName() => "Day 17";

        public async Task<long> One()
        {
            var data = await Common.ReadFile(_fileDayName, "One");
            int registerA = int.Parse(data[0].Split(':')[1]);
            int registerB = int.Parse(data[1].Split(':')[1]);
            int registerC = int.Parse(data[2].Split(':')[1]);

            List<int> program = data[4].Split(':')[1].Trim().Split(',').Select(s => int.Parse(s)).ToList();

            Console.WriteLine(ProcessData(registerA, registerB, registerC, program));
            return 0;
        }

        public async Task<long> Two()
        {
            var data = await Common.ReadFile(_fileDayName, "Two");
            int count = 0;
            return count;
        }

        private string ProcessData(int registerA, int registerB, int registerC, List<int> program)
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

        private int GetComboOperand(int operand, int registerA, int registerB, int registerC)
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

        private void ProcessAdv(ref int registerA, int operand, int registerB, int registerC)
        {
            registerA = (int)(registerA / (Math.Pow(2, GetComboOperand(operand, registerA, registerB, registerC))));
        }
        private void ProcessBxl(ref int registerB, int operand)
        {
            registerB = registerB ^ operand;
        }

        private void ProcessBst(ref int registerB, int operand, int registerA, int registerC)
        {
            registerB = GetComboOperand(operand, registerA, registerB, registerC) % 8;
        }

        //A return of true means the pointer shouldn't move forward after
        private bool ProcessJnz(int registerA, int operand, ref int currentPointerIndex)
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

        private void ProcessBxc(ref int registerB, int registerC)
        {
            registerB = registerB ^ registerC;
        }

        private string ProcessOut(int operand, int registerA, int registerB, int registerC)
        {
            var output = GetComboOperand(operand, registerA, registerB, registerC) % 8;
            return output.ToString();
        }

        private void ProcessBdv(int registerA, ref int registerB, int operand, int registerC)
        {
            registerB = (int)(registerA / (Math.Pow(2, GetComboOperand(operand, registerA, registerB, registerC))));
        }

        private void ProcessCdv(int registerA, ref int registerC, int operand, int registerB)
        {
            registerC = (int)(registerA / (Math.Pow(2, GetComboOperand(operand, registerA, registerB, registerC))));
        }
    }
}
