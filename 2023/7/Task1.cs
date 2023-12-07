using System.Text.RegularExpressions;

public class Task1
{
    private string[] _input;
    public Task1(string[] input)
    {
        _input = input;
    }
    private List<Hand> GetHands()
    {
        return _input.Select(line =>
        {
            var split = line.Split(" ");
            return (new Hand()
            {
                Cards = split[0].Select(c => c.ToString()).Select(card =>
                {
                    return card switch
                    {
                        "A" => 14,
                        "K" => 13,
                        "Q" => 12,
                        "J" => 11,
                        "T" => 10,
                        _ => int.Parse(card),
                    };
                }).ToList(),
                Bid = int.Parse(split[1])
            });
        }).ToList();
    }
    public void Solve()
    {
        var hands = GetHands();
        hands.Sort(new HandComparer());

        var result = Enumerable.Range(1, hands.Count).Sum(i => hands[i-1].Bid *  i);
        Console.WriteLine(result);
    }

    private class Hand
    {
        public List<int> Cards;
        public int Bid;
    }
    public class HandComparer : IComparer<Hand>
    {
        private int GetTypeStrength(Hand hand)
        {
            var instancesOf = new Dictionary<int, int>();
            foreach (var card in hand.Cards)
            {
                if (!instancesOf.ContainsKey(card))
                {
                    instancesOf[card] = 0;
                }
                instancesOf[card]++;
            }
            if (instancesOf.Values.Any(val => val == 5)) return 7; //five of a kind
            if (instancesOf.Values.Any(val => val == 4)) return 6; //four of a kind
            if (instancesOf.Values.Any(val => val == 3) && instancesOf.Values.Any(val => val == 2)) return 5; //full house
            if (instancesOf.Values.Any(val => val == 3)) return 4; //three of a kind

            var firstPairKey = instancesOf.FirstOrDefault(inst => inst.Value == 2).Key;
            if (firstPairKey != default && instancesOf.Any(inst => inst.Value == 2 && inst.Key != firstPairKey)) return 3; //two pairs
            if (firstPairKey != default) return 2; //one pair
            if (!instancesOf.Values.Any(val => val > 1)) return 1; //high card
            return 0;
        }
        int IComparer<Hand>.Compare(Hand x, Hand y)
        {
            var xTypeStrength = GetTypeStrength(x);
            var yTypeStrength = GetTypeStrength(y);
            if (xTypeStrength == yTypeStrength)
            {
                for (var i = 0; i < x.Cards.Count; i++)
                {
                    if (x.Cards[i] == y.Cards[i]) continue;
                    return (x.Cards[i] > y.Cards[i]) ? 1 : -1 ;
                }
                return 0; //equal
            }
            return xTypeStrength>yTypeStrength ? 1 : -1;
        }
    }
}