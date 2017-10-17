using System;
using System.Collections.Generic;
using System.Text;

namespace KnapSacks.Capsules
{
    public class Item : IEquatable<Item>
    {

        public static IComparer<Item> WeightComparer = new WeightComparator();
        public static IComparer<Item> ValueComparer = new ValueComparator();
        public static IComparer<Item> RatioComparer = new RatioComparator();

        public static Item Default = null;

        public double Weight { get; set; } = 0.0d;

        public double Value { get; set; } = 0.0d;

        public double Ratio { get; set; } = 0.0d;

        public int Id { get; private set; } = 0;

        public bool Included { get; set; } = false;

        public Item(int id, double weight, double value)
        {
            this.Id = id;
            this.Weight = weight;
            this.Value = value;
            this.Ratio = (Value / Weight);
        }

        public bool Equals(Item other)
        { 
            if (this.Weight == other.Weight &&
                this.Value == other.Value &&
                this.Ratio == other.Ratio &&
                this.Equals(other)){
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Value: {Value} Weight: {Weight} Ratio: {Ratio}";
        }

        public string ToFileString()
        {
            return $"{Id}|{Weight}|{Value}";
        }

        private class WeightComparator : IComparer<Item>
        {
            public int Compare(Item one, Item two) => (int)(one.Weight - two.Weight);
        }

        private class ValueComparator : IComparer<Item>
        {
            public int Compare(Item one, Item two) => (int)(one.Value - two.Value);
        }

        private class RatioComparator : IComparer<Item>
        {
            public int Compare(Item one, Item two) => (int)(one.Ratio - two.Ratio);
        }
    }
}
