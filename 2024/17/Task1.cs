using CommonLib;
using System.Text.RegularExpressions;

public class Task1(string[] input) : BaseTask()
{
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

        var pointer = 0;
        var output = new List<int>();
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
        Console.WriteLine(string.Join(",", output));
    }
    class Computor(Dictionary<char, long> registry)
    {
        private readonly Dictionary<char, long> registry = registry;

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
            registry[outputRegistry] = (long)Math.Floor(registry['A'] / Math.Pow(2, GetComboOperand(literalOperand)));
            return new OperationResult(2);
        }
        OperationResult Bxl(int literalOperand)
        {
            registry['B'] = registry['B'] ^ literalOperand;
            return new OperationResult(2);
        }
        OperationResult Bst(int literalOperand)
        {
            registry['B'] = GetComboOperand(literalOperand) % 8;
            return new OperationResult(2);
        }
        OperationResult Jnz(int literalOperand)
        {
            if (registry['A'] == 0) return new OperationResult(2);
            return new OperationResult(literalOperand)
            {
                InstructionPointerMode = OperationResult.InstructionPointerModeType.Fixed
            };
        }
        OperationResult Bxc()
        {
            registry['B'] = registry['B'] ^ registry['C'];
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
            if (literalOperand == 4) return registry['A'];
            if (literalOperand == 5) return registry['B'];
            if (literalOperand == 6) return registry['C'];
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