using CommonLib;
using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var binaryString = string.Concat(input[0].Select(c =>Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

        var result = ParsePacket(0);

        return result.Value;

        (int NextIndex, long Value) ParsePacket(int startIndex)
        {
            var currentIndex = startIndex;

            var version = Convert.ToInt32(binaryString.Substring(currentIndex, 3), 2);
            var typeId = Convert.ToInt32(binaryString.Substring(currentIndex + 3, 3), 2);

            currentIndex += 6;

            if (typeId == 4) // literal packet
            {
                var literalBits = "";

                while (true)
                {
                    var isLastGroup = binaryString[currentIndex] == '0';
                    currentIndex++;

                    literalBits += binaryString.Substring(currentIndex, 4);
                    currentIndex += 4;

                    if (isLastGroup)
                    {
                        break;
                    }
                }

                var literalValue = Convert.ToInt64(literalBits, 2);
                return (currentIndex, literalValue);
            }

            // operator packet
            var lengthTypeId = binaryString[currentIndex];
            currentIndex++;

            var subPacketValues = new List<long>();

            if (lengthTypeId == '0')
            {
                // next 15 bits = total bit length of all sub-packets
                var subPacketBitLength = Convert.ToInt32(binaryString.Substring(currentIndex, 15), 2);
                currentIndex += 15;

                var subPacketsEndIndex = currentIndex + subPacketBitLength;

                while (currentIndex < subPacketsEndIndex)
                {
                    var subPacket = ParsePacket(currentIndex);
                    currentIndex = subPacket.NextIndex;
                    subPacketValues.Add(subPacket.Value);
                }
            }
            else
            {
                // next 11 bits = number of sub-packets
                var subPacketCount = Convert.ToInt32(binaryString.Substring(currentIndex, 11), 2);
                currentIndex += 11;

                for (var i = 0; i < subPacketCount; i++)
                {
                    var subPacket = ParsePacket(currentIndex);
                    currentIndex = subPacket.NextIndex;
                    subPacketValues.Add(subPacket.Value);
                }
            }

            long value = typeId switch
            {
                0 => subPacketValues.Sum(),
                1 => subPacketValues.Aggregate(1L, (acc, x) => acc * x),
                2 => subPacketValues.Min(),
                3 => subPacketValues.Max(),
                5 => subPacketValues[0] > subPacketValues[1] ? 1 : 0,
                6 => subPacketValues[0] < subPacketValues[1] ? 1 : 0,
                7 => subPacketValues[0] == subPacketValues[1] ? 1 : 0,
                _ => throw new InvalidOperationException($"Unknown type ID: {typeId}")
            };

            return (currentIndex, value);
        }
    }
}