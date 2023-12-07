using System.Text.RegularExpressions;

public class Task2
{
    private static int _joker = 1;
    private string[] _input;
    public Task2(string[] input)
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
                        "A" => 13,
                        "K" => 12,
                        "Q" => 11,
                        "J" => _joker,
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

        var result = Enumerable.Range(1, hands.Count).Sum(i => hands[i - 1].Bid * i);
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
            instancesOf.TryGetValue(_joker, out var jokers);
            instancesOf.Remove(_joker);

            if (jokers == 5 || instancesOf.Values.Any(val => val >= 5 - jokers)) return 7; //five of a kind
            if (instancesOf.Values.Any(val => val >= 4 - jokers)) return 6; //four of a kind

            var threeOfAKind = instancesOf.FirstOrDefault(inst => inst.Value >= 3 - jokers);
            if (threeOfAKind.Key != default)
            {
                var remainingJokers = jokers - (3 - threeOfAKind.Value);
                if (instancesOf.Any(inst => inst.Key != threeOfAKind.Key && inst.Value >= 2 - remainingJokers)) return 5; //full house
                return 4; //three of a kind
            }

            var firstPair = instancesOf.FirstOrDefault(inst => inst.Value >= 2 - jokers);
            if (firstPair.Key != default)
            {
                var remainingJokers = jokers - (2 - firstPair.Value);
                if (instancesOf.Any(inst => inst.Key != firstPair.Key && inst.Value >= 2 - remainingJokers)) return 3; //two pairs
                return 2; //one pair
            }
            if (instancesOf.Values.Count(val => val > 1) < jokers) return 1; //high card
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
                    return (x.Cards[i] > y.Cards[i]) ? 1 : -1;
                }
                return 0; //equal
            }
            return xTypeStrength > yTypeStrength ? 1 : -1;
        }
    }
}