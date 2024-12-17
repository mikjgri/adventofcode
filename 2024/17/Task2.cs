using CommonLib;
using System.Text.RegularExpressions;

public class Task2(string[] input) : BaseTask()
{
    //this code works... It probably won't finish running in your lifetime, but it does work!
    protected override void Solve()
    {
        List<int> getNumbers(string line)
        {
            return Regex.Matches(line, "\\d+").Select(m => int.Parse(m.Value)).ToList();
        }
        var computor = new Computor(new Dictionary<char, long>
        {
            ['A'] = getNumbers(input[0]).First(),
            ['B'] = getNumbers(input[1]).First(),
            ['C'] = getNumbers(input[2]).First()
        });
        var program = getNumbers(input[4]);

        var output = new List<int>();
        long i = -1;
        while (!program.SequenceEqual(output))
        {
            i++;
            computor.Registry['A'] = i;
            var pointer = 0;
            output = [];
            while (pointer < program.Count)
            {
                var operationResult = computor.Computorize(program[pointer], program[pointer + 1]);
                if (operationResult.Output.HasValue)
                {
                    output.Add(operationResult.Output.Value);
                }
                pointer = operationResult.InstructionPointerMode switch
                {
                    Computor.OperationResult.InstructionPointerModeType.Relative => pointer += operationResult.InstructionPointer,
                    Computor.OperationResult.InstructionPointerModeType.Fixed => pointer = operationResult.InstructionPointer,
                    _ => throw new NotImplementedException()
                };
            }
        }
        Console.WriteLine(i);
    }
    class Computor
    {
        public Dictionary<char, long> Registry;
        public Computor(Dictionary<char, long> registry)
        {
            Registry = registry;
        }

        public OperationResult Computorize(int opcode, int literalOperand)
        {
            return opcode switch
            {
                0 => Xdv('A', literalOperand),
                1 => Bxl(literalOperand),
                2 => Bst(literalOperand),
                3 => Jnz(literalOperand),
                4 => Bxc(),
                5 => Out(literalOperand),
                6 => Xdv('B', literalOperand),
                7 => Xdv('C', literalOperand),
                _ => throw new NotImplementedException(),
            };
        }
        OperationResult Xdv(char outputRegistry, int literalOperand)
        {
            Registry[outputRegistry] = (long)Math.Floor(Registry['A'] / Math.Pow(2, GetComboOperand(literalOperand)));
            return new OperationResult(2);
        }
        OperationResult Bxl(int literalOperand)
        {
            Registry['B'] = Registry['B'] ^ literalOperand;
            return new OperationResult(2);
        }
        OperationResult Bst(int literalOperand)
        {
            Registry['B'] = GetComboOperand(literalOperand) % 8;
            return new OperationResult(2);
        }
        OperationResult Jnz(int literalOperand)
        {
            if (Registry['A'] == 0) return new OperationResult(2);
            return new OperationResult(literalOperand)
            {
                InstructionPointerMode = OperationResult.InstructionPointerModeType.Fixed
            };
        }
        OperationResult Bxc()
        {
            Registry['B'] = Registry['B'] ^ Registry['C'];
            return new OperationResult(2);
        }
        OperationResult Out(int literalOperand)
        {
            return new OperationResult(2)
            {
                Output = (int)(GetComboOperand(literalOperand) % 8)
            };
        }
        long GetComboOperand(int literalOperand)
        {
            if (literalOperand >= 0 && literalOperand <= 3) return literalOperand;
            if (literalOperand == 4) return Registry['A'];
            if (literalOperand == 5) return Registry['B'];
            if (literalOperand == 6) return Registry['C'];
            throw new ArgumentException("Invalid program");
        }
        public class OperationResult(int instructionPointer)
        {
            public readonly int InstructionPointer = instructionPointer;
            public int? Output;
            public InstructionPointerModeType InstructionPointerMode = InstructionPointerModeType.Relative;
            public enum InstructionPointerModeType
            {
                Relative,
                Fixed
            }
        }
    }
}