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

        public int Count => entries.Count;
        
        public List<WeightedListEntry<T>> GetAllEntries()
        {
            return entries;
        }
        
        public WeightedList()
        {
            entries = new List<WeightedListEntry<T>>();
        }

        public WeightedList(WeightedList<T> list)
        {
            entries = new List<WeightedListEntry<T>>();
            foreach (var entry in list.GetAllEntries())
            {
                Add(entry);
            }
        }

        public void Add(WeightedListEntry<T> value)
        {
            entries.Add(value);
        }
        
        public T GetRandom(bool removeOnGet = false)
        {
            int totalWeight = entries.Sum(x => x.Weight);
            int targetWeight = Random.Range(0, totalWeight);
            foreach (var entry in entries)
            {
                targetWeight -= entry.Weight;
                if (targetWeight < 0)
                {
                    if (removeOnGet)
                    {
                        entries.Remove(entry);
                    }
                    return entry.Value;
                }
            }
            return default;
        }
    }
    
    [Serializable] 
    public struct WeightedListEntry<T> : IEquatable<WeightedListEntry<T>>
    {
        public T Value;
        public int Weight;

        public bool Equals(WeightedListEntry<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is WeightedListEntry<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Weight);
        }
    }
}
