using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlocksGame
{
    [Serializable]
    public class WeightedList<T>
    {
        [SerializeField] private List<WeightedListEntry<T>> entries = new();

        public T GetRandom()
        {
            int totalWeight = entries.Sum(x => x.Weight);
            int targetWeight = Random.Range(0, totalWeight);
            foreach (var entry in entries)
            {
                targetWeight -= entry.Weight;
                if (targetWeight < 0)
                {
                    return entry.Value;
                }
            }
            return default(T);
        }
    }
    
    [Serializable] 
    public struct WeightedListEntry<T>
    {
        public T Value;
        public int Weight;
    }
}
