using CommonLib.Solvers;
using System.Text.RegularExpressions;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var machines = input.Select(line =>
        {
            var lightGoalState = Regex.Match(line, @"\[([.#]+)\]").Groups[1].Value.Select(c => c == '#').ToArray();
            var buttons = Regex.Matches(line, @"\((\d+(?:\s*,\s*\d+)*)\)").Select(match =>
            {
                var split = match.Groups[1].Value.Split(",");
                return new Button([.. split.Select(s => int.Parse(s))]);
            }).ToArray();
            return new Machine(lightGoalState, buttons);
        }).ToList();

        return machines.Sum(machine =>
        {
            int? bestFlipCount = null;
            var dynamicFailOutValue = 1;
            while (bestFlipCount == null)
            {
                FlipSwitches([.. Enumerable.Range(0, machine.LightGoalState.Length).Select(i => false)], 0);
                dynamicFailOutValue++;
            }
            Console.WriteLine($"{machines.IndexOf(machine) + 1}/{machines.Count} - {bestFlipCount.Value}");
            return bestFlipCount;

            void FlipSwitches(bool[] state, int flipCount, Button? pressedButton = null)
            {
                if (pressedButton != null)
                {
                    foreach (var modifier in pressedButton.Modifiers)
                    {
                        state[modifier] = !state[modifier];
                    }
                    flipCount++;
                }
                if (flipCount > dynamicFailOutValue) return;
                if (bestFlipCount != null && flipCount > bestFlipCount) return;
                if (state.SequenceEqual(machine.LightGoalState))
                {
                    if (bestFlipCount == null || flipCount < bestFlipCount) bestFlipCount = flipCount;
                    return;
                }

                foreach (var nextButton in machine.Buttons)
                {
                    if (nextButton == pressedButton) continue; //dont press the same twice in a row
                    FlipSwitches([.. state], flipCount, nextButton);
                }
            }
        })!;
    }
    record Machine(bool[] LightGoalState, Button[] Buttons);
    record Button(int[] Modifiers);
}