using KnapSacks.Capsules;
using System;
using System.Collections.Generic;
namespace KnapSacks.Solvers
{
    public class Node : IComparable<Node>
    {

        public static IComparer<Node> Comparer = new NodeComparer();

        public List<Item> Included { get; } = new List<Item>();

        public double valueBound { get; set; } = 0;
        public double Value { get; set; } = 0;
        public double Weight { get; set; } = 0;

        public int Index { get; set; } = 0;

        public Node() { }

        public Node(Node parent)
        {
            Index = parent.Index + 1;
            valueBound = parent.valueBound;
            Value = parent.Value;
            Weight = parent.Weight;
            Included = new List<Item>(parent.Included);
        }

        public int CompareTo(Node other) => (int)(other.valueBound - valueBound);

        public void ComputeBound(List<Item> items, Bag bag)
        {
            valueBound = Value;

            var index = Index;
            var weightBound = Weight;
            var item = Item.Default;

            do
            {
                item = items[index];

                if (weightBound + item.Weight > bag.RemainingCapacity)
                {
                    break;
                }

                weightBound += item.Weight;
                valueBound += item.Value;

                index++;

            } while (index < items.Count);

            valueBound += (bag.RemainingCapacity - weightBound) * (item.Value / item.Weight);
        }
    }

    public class NodeComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y) => (int)(y.valueBound - x.valueBound);
    }
}
