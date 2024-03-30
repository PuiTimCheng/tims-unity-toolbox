using System.Collections;
using System.Collections.Generic;

namespace MatchThreeRoguelike
{
    public class RandomSeedGenerator
    {
        public static string GenerateStringSeed(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Add lowercase if needed
            var random = new System.Random();
            var randomString = new char[length];
            for (int i = 0; i < length; i++)
            {
                randomString[i] = chars[random.Next(chars.Length)];
            }
            return new string(randomString);
        }

        public static int GetSeedFromString(string seed) {
            return seed.GetHashCode();
        }
    }
}
