using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace TimToolBox.Extensions {
    public static class RandomExtensions {
        public static T RandomPickOne<T>(this List<T> list, Random random = null) {
            var rng = random == null ? new Random() : random;
            if (list.Count == 0) {
                throw new InvalidOperationException("Cannot select a random element from an empty list.");
            }

            return list[rng.Next(list.Count)];
        }

        public static T RandomPickOne<T>(this T[] array, Random random = null) {
            var rng = random == null ? new Random() : random;
            if (array.Length == 0) {
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
        public static bool RandomSuccess(this float probability, Random random = null) {
            float roll;
            if (random != null) {
                // Use the provided System.Random instance to generate a random value between 0.0 and 1.0
                roll = (float) random.NextDouble();
            }
            else {
                // Use Unity's global random generator
                roll = UnityEngine.Random.value;
            }

            // Return true if the roll is within the probability
            return roll <= probability;
        }

        public static ICollection<T> RandomSelectWeightedItems<T>(this ICollection<T> items, int n, Func<T, int> weightSelector,
                                                                              Random random = null) {
            // Use provided random instance or create a new one if not provided
            random ??= new Random();
            // Calculate the total weight
            int totalWeight = items.Sum(weightSelector);
            // List to store the selected items
            List<T> selectedItems = new List<T>();
            for (int i = 0; i < n; i++) {
                int randomNumber = random.Next(totalWeight);
                int cumulativeWeight = 0;

                foreach (var item in items) {
                    cumulativeWeight += weightSelector(item);
                    if (randomNumber < cumulativeWeight) {
                        selectedItems.Add(item);
                        break;
                    }
                }
            }

            return selectedItems;
        }

        public static ICollection<T> RandomSelectNonReplacingWeightedItems<T>(this ICollection<T> items, int n, Func<T, int> weightSelector,
                                                   Random random = null) {
            if (n > items.Count) {
                throw new ArgumentException("n cannot be greater than the number of items in the list");
            }

            // Use provided random instance or create a new one if not provided
            random ??= new Random();

            // Create a copy of the list to avoid modifying the original
            List<T> itemsCopy = new List<T>(items);

            // Calculate the total weight
            int totalWeight = itemsCopy.Sum(weightSelector);

            // List to store the selected items
            List<T> selectedItems = new List<T>();

            for (int i = 0; i < n; i++) {
                int randomNumber = random.Next(totalWeight);
                int cumulativeWeight = 0;

                foreach (var item in itemsCopy) {
                    cumulativeWeight += weightSelector(item);
                    if (randomNumber < cumulativeWeight) {
                        selectedItems.Add(item);
                        totalWeight -= weightSelector(item);
                        itemsCopy.Remove(item);
                        break;
                    }
                }
            }

            return selectedItems;
        }
    }
}