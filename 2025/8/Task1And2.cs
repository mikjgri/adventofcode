using CommonLib;

public class Task1And2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var task1_iterations = input.Length > 20 ? 1000 : 10; //demoset or not
        List<JunctionBox> junctionBoxes = [.. input.Select(line =>
        {
            var s = line.Split(",");
            return new JunctionBox(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
        })];

        var junctionBoxPairs = junctionBoxes
            .SelectMany(jbA => junctionBoxes
                .Where(jbB => jbB != jbA)
                .Select(jbB => new JunctionBoxPair(jbA, jbB)))
            .Distinct()
            .OrderBy(jbP => jbP.EuclideanDistance)
            .ToList();

        var circuits = junctionBoxes.Select(jb => new List<JunctionBox>() { jb }).ToList();

        foreach (var jbP in junctionBoxPairs)
        {
            if(junctionBoxPairs.IndexOf(jbP) == task1_iterations)
            {
                var ht = circuits.Select(c => c.Count).OrderDescending().Take(3).ToList();
                Console.WriteLine($"Task 1: {ht[0] * ht[1] * ht[2]}");
            }

            var cA = circuits.FirstOrDefault(c => c.Contains(jbP.A));
            var cB = circuits.FirstOrDefault(c => c.Contains(jbP.B));

            if (cA != null && cB != null)
            {
                if (cA != cB) //different circuits. merge
                {
                    cA.AddRange(cB.Where(jb => !cA.Contains(jb)));
                    circuits.Remove(cB);
                    if (circuits.Count == 1)
                    {
                        Console.WriteLine($"Task 2: {(long)jbP.A.X * jbP.B.X}");
                        break;
                    }
                }
                //else same circit
                continue;
            }
            if (cA != null) //add to A circuit
            {
                cA.Add(jbP.B);
                continue;
            }
            if (cB != null) //add to B circuit
            {
                cB.Add(jbP.A);
                continue;
            }
            circuits.Add([jbP.A, jbP.B]);
        }
    }
    record JunctionBox(int X, int Y, int Z);

    class JunctionBoxPair(JunctionBox a, JunctionBox b) : IEquatable<JunctionBoxPair>
    {
        public JunctionBox A = a;
        public JunctionBox B = b;

        public double EuclideanDistance { get => Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2) + Math.Pow(A.Z - B.Z, 2)); }
        public bool Equals(JunctionBoxPair? other)
        {
            return (other!.A == A && other.B == B) || (other!.A == B && other.B == A);
        }
        public override bool Equals(object? obj) => Equals(obj as JunctionBoxPair);
        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }
    }
}
