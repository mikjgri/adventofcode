using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var dialPosition = 50;
        var instructions = input.Select(line => int.Parse(line[1..]) * (line.First() == 'L' ? -1 : 1));

        var sum = instructions.Sum(instr =>
        {
            var isClicky = 0;
            var isNeg = instr < 0;
            for (var i = 0; i<Math.Abs(instr); i++)
            {
                //Lame solution, but brain herp derp
                if (isNeg) 
                    dialPosition--;
                else 
                    dialPosition++;

                if (dialPosition < 0) 
                    dialPosition = 99;
                if (dialPosition > 99) 
                    dialPosition = 0;

                if (dialPosition == 0) 
                    isClicky++;
            }
            return isClicky;
        });
        Console.WriteLine(sum);
    }
}