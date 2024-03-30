using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace TimToolBox.Extensions
{
    public static class RandomExtensions
    {
        public static T RandomPickOne<T>(this List<T> list, Random random = null)
        {
            var rng = random == null ? new Random() : random;
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Cannot select a random element from an empty list.");
            }

            return list[rng.Next(list.Count)];
        }

        public static T RandomPickOne<T>(this T[] array, Random random = null)
        {
            var rng = random == null ? new Random() : random;
            if (array.Length == 0)
            {
                throw new InvalidOperationException("Cannot select a random element from an empty array.");
            }

            return array[rng.Next(array.Length)];
        }
        
        /// <summary>
        /// Determines if a random success condition is met based on the specified probability.
        /// Optionally uses a provided Random object or Unity's global random generator by default.
        /// </summary>
        /// <param name="probability">The probability of success, expressed as a float between 0.0 and 1.0.</param>
        /// <param name="random">An optional System.Random instance for generating the random value. If null, Unity's global random generator is used.</param>
        /// <returns>True if the random condition is met, false otherwise.</returns>
        public static bool RandomSuccess(this float probability, Random random = null)
        {
            float roll;
            if (random != null)
            {
                // Use the provided System.Random instance to generate a random value between 0.0 and 1.0
                roll = (float)random.NextDouble();
            }
            else
            {
                // Use Unity's global random generator
                roll = UnityEngine.Random.value;
            }

            // Return true if the roll is within the probability
            return roll <= probability;
        }
    }
}