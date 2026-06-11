using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var res = 0;
        foreach (var item in input)
        {
            var s1 = item.Split("|", StringSplitOptions.TrimEntries);
            var inputs = s1[0].Split(" ").Select(s => AlphabeticallySort(s)).ToArray();
            var outputs = s1[1].Split(" ").Select(s => AlphabeticallySort(s)).ToArray();

            var allSignals = inputs.Concat(outputs).Distinct().ToList();

            var numberDict = new Dictionary<int, string>();
            var signalPositions = new Dictionary<SignalPositions, char>();

            //easy numbers
            PopulateDictAndRemoveEasy(1, 2);
            PopulateDictAndRemoveEasy(4, 4);
            PopulateDictAndRemoveEasy(7, 3);
            PopulateDictAndRemoveEasy(8, 7);

            //number 3 is the 5 signal with both from 1
            PopulateDictAndRemoveHard(3, allSignals.Single(s => s.Length == 5 && ContainsAllFrom(s, numberDict[1])));
            //number 6 is the 6 signal with one missing from 1
            PopulateDictAndRemoveHard(6, allSignals.Single(s => s.Length == 6 && !ContainsAllFrom(s, numberDict[1])));

            //the one in 8 that is not in 3 and 4 combined is the lowerLeft
            var threeAndFourCombined = numberDict[3].ToCharArray().Concat(numberDict[4].ToCharArray()).Distinct();
            signalPositions.Add(SignalPositions.LowerLeft, numberDict[8].Single(p => !threeAndFourCombined.Contains(p)));

            //the one in 1 that is missing from 6 is upperRight
            signalPositions.Add(SignalPositions.UpperRight, numberDict[1].Single(p => !numberDict[6].Any(n6 => n6 == p)));

            //9 is the 6 signal that contains all from 8 except lower left
            PopulateDictAndRemoveHard(9, allSignals.Single(s => s.Length == 6 && s.All(p => numberDict[8].ToCharArray().Where(n8 => n8 != signalPositions[SignalPositions.LowerLeft]).ToList().Contains(p))));

            //5 is the 5 signal that contains all from 6 except upper right
            PopulateDictAndRemoveHard(5, allSignals.Single(s => s.Length == 5 && s.All(p => numberDict[6].ToCharArray().Where(n6 => n6 != signalPositions[SignalPositions.UpperRight]).ToList().Contains(p))));

            //8 - 3 - lower left = upper left
            signalPositions.Add(SignalPositions.UpperLeft, numberDict[8].Single(n8 => n8 != signalPositions[SignalPositions.LowerLeft] && !numberDict[3].Any(n3 => n3 == n8)));

            //middle is the one from 4 that is not in 1 and not top left
            signalPositions.Add(SignalPositions.Middle, numberDict[4].Single(n4 => n4 != signalPositions[SignalPositions.UpperLeft] && !numberDict[1].Any(n1 => n1 == n4)));

            //0 is the 6 signal that contains all from 8 except middle
            PopulateDictAndRemoveHard(0, allSignals.Single(s => s.Length == 6 && s.All(p => numberDict[8].ToCharArray().Where(n8 => n8 != signalPositions[SignalPositions.Middle]).ToList().Contains(p))));

            //2 is the last one left
            PopulateDictAndRemoveHard(2, allSignals.Single());


            var subSum = "";
            foreach (var output in outputs)
            {
                subSum += numberDict.First(nd => nd.Value == output).Key;
            }
            res += int.Parse(subSum);

            void PopulateDictAndRemoveEasy(int digit, int signalLength)
            {
                var d = allSignals.First(s => s.Length == signalLength);
                numberDict.Add(digit, d);
                allSignals.Remove(d);
            }
            void PopulateDictAndRemoveHard(int digit, string signal)
            {
                numberDict.Add(digit, signal);
                allSignals.Remove(signal);
            }
        }
        return res;
    }
    string AlphabeticallySort(string input)
    {
        return string.Concat(input.Select(c => c).Order());
    }
    bool ContainsAllFrom(string input, string from)
    {
        return from.All(f => input.ToCharArray().Contains(f));
    }
    enum SignalPositions
    {
        UpperTop,
        UpperLeft,
        UpperRight,
        Middle,
        LowerBottom,
        LowerLeft,
        LowerRight
    }
}