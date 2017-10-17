using KnapSacks.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapSacks.Capsules
{
    public class Bag : IEquatable<Bag>
    {
        public int Id { get; private set; } = 0;

        public int ItemCount { get; private set; } = 0;

        public double WeightCapacity { get; private set; } = int.MaxValue;

        public double AccumulatedWeight { get; private set; } = 0d;
        public double AccumulatedValue { get; private set; } = 0d;

        public List<Item> Items { get; private set; } = new List<Item>();

        public Bag(int id, double weightCapacity)
        {
            this.Id = id;
            this.WeightCapacity = weightCapacity;
        }

        public void AddItem(Item item)
        {
            this.AccumulatedValue += item.Value;
            this.AccumulatedWeight += item.Weight;

            this.Items.Add(item);

            this.ItemCount++;
        }

        public void AddItems(params Item[] items)
        {
            foreach(Item item in items)
            {
                AddItem(item);
            }
        }

        public bool RemoveItem(Item item)
        {
            this.AccumulatedValue -= item.Value;
            this.WeightCapacity -= item.Weight;

            this.ItemCount--;

            return Items.Remove(item);
        }

        public void RemoveAllItems(params Item[] items)
        {
            foreach (Item item in items)
            {
                RemoveItem(item);
            }
        }

        internal bool IsAtMaxCapacity => Utility.GetTotalWeight(Items) >= WeightCapacity;
        

        public double RemainingCapacity => WeightCapacity - AccumulatedWeight;

        public string ToFileString()
        {
            return $"{Id}|{WeightCapacity}";
        }

        public override string ToString()
        {
            return $"Max Weight: {WeightCapacity} Current Weigth: {AccumulatedWeight} Current Value: {AccumulatedValue}";
        }

        public bool Equals(Bag other)
        {
            if (Id != other.Id) return false;

            if (WeightCapacity != other.WeightCapacity) return false;

            if (AccumulatedWeight != other.AccumulatedWeight) return false;

            if (AccumulatedValue != other.AccumulatedValue) return false;

            if (Items.Count != other.Items.Count) return false;

            return true;
        }
    }
}
