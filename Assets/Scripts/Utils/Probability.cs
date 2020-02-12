using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probability {
    // TODO: try to replace T with a string and another function to map the type to string due that the GameObject type is already very memory consuming, storing as a key will cost more memory
    public static T RandomEventsWithProb<T>(
        List<KeyValuePair<T, float>> items, float totalProb) {
        // pick random value with in range the sum of all occurence probabilities
        float randomValue = Random.Range(0, totalProb);
        float cumulative  = 0;

        foreach (KeyValuePair<T, float> item in items) {
            cumulative += item.Value;
            if (randomValue < cumulative) {
                return item.Key;
            }
        }

        return default;
    }
}