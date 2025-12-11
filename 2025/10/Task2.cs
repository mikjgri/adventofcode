using CommonLib;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class Task2(string[] input) : BaseTask()
{
    //works like a charm if you have a couple of years to run it
    protected override object Solve()
    {
        var machines = input.Select(line =>
        {
            var joltageGoalState = Regex.Match(line, @"\{([^{}]*)\}").Groups[1].Value.Split(",").Select(c => int.Parse(c)).ToArray();
            var buttons = Regex.Matches(line, @"\((\d+(?:\s*,\s*\d+)*)\)").Select(match =>
            {
                var split = match.Groups[1].Value.Split(",");
                return new Button([.. split.Select(s => int.Parse(s))]);
            }).OrderByDescending(b => b.Modifiers.Sum()).ToArray();
            return new Machine(joltageGoalState, buttons);
        }).ToList();

        return machines.Sum(machine =>
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int? bestFlipCount = null;
            var dynamicFailOutValue = 1;
            while (bestFlipCount == null)
            {
                FlipSwitches([.. Enumerable.Range(0, machine.JoltageGoalState.Length).Select(i => 0)], 0);
                Console.WriteLine($"DynamicFailout = {dynamicFailOutValue}, Time = {stopWatch.ElapsedMilliseconds}ms");
                dynamicFailOutValue++;
            }
            Console.WriteLine($"{machines.IndexOf(machine) + 1}/{machines.Count}, Result = {bestFlipCount.Value}, DynamicFailOut= {dynamicFailOutValue-1}");
            return bestFlipCount;

            void FlipSwitches(int[] state, int flipCount, Button? pressedButton = null)
            {
                if (pressedButton != null)
                {
                    foreach (var modifier in pressedButton.Modifiers)
                    {
                        state[modifier]++;
                    }
                    flipCount++;
                }
                if (flipCount > dynamicFailOutValue) return;

                if (bestFlipCount != null && flipCount > bestFlipCount) return;
                if (state.SequenceEqual(machine.JoltageGoalState))
                {
                    if (bestFlipCount == null || flipCount < bestFlipCount) bestFlipCount = flipCount;
                    return;
                }

                foreach (var nextButton in machine.Buttons)
                {
                    var wouldOverflow = false;
                    foreach (var modifier in nextButton.Modifiers)
                    {
                        if (state[modifier]+1 > machine.JoltageGoalState[modifier])
                        {
                            wouldOverflow = true; break;
                        }
                    }
                    if (wouldOverflow) continue;
                    FlipSwitches([.. state], flipCount, nextButton);
                }
            }
        })!;
    }
    record Machine(int[] JoltageGoalState, Button[] Buttons);
    record Button(int[] Modifiers);
}