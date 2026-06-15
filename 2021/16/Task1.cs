using CommonLib;
using CommonLib.Solvers;
using System.Reflection.Metadata.Ecma335;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var binaryString = string.Concat(input[0].Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        var versionSum = 0;

        ParsePacket(0);

        int ParsePacket(int startIndex)
        {
            var currentIndex = startIndex;

            var version = Convert.ToInt32(binaryString.Substring(currentIndex, 3), 2);
            var typeId = Convert.ToInt32(binaryString.Substring(currentIndex + 3, 3), 2);

            currentIndex += 6;
            versionSum += version;

            if (typeId == 4) // literal packet
            {
                while (true)
                {
                    var isLastGroup = binaryString[currentIndex] == '0';
                    currentIndex += 5;

                    if (isLastGroup)
                    {
                        break;
                    }
                }

                return currentIndex;
            }

            // operator packet
            var lengthTypeId = binaryString[currentIndex];
            currentIndex++;

            if (lengthTypeId == '0')
            {
                // next 15 bits = total bit length of all sub-packets
                var subPacketBitLength = Convert.ToInt32(binaryString.Substring(currentIndex, 15), 2);
                currentIndex += 15;

                var subPacketsEndIndex = currentIndex + subPacketBitLength;

                while (currentIndex < subPacketsEndIndex)
                {
                    currentIndex = ParsePacket(currentIndex);
                }

                return currentIndex;
            }
            else
            {
                // next 11 bits = number of sub-packets
                var subPacketCount = Convert.ToInt32(binaryString.Substring(currentIndex, 11), 2);
                currentIndex += 11;

                for (var i = 0; i < subPacketCount; i++)
                {
                    currentIndex = ParsePacket(currentIndex);
                }

                return currentIndex;
            }
        }

        return versionSum;
    }
}