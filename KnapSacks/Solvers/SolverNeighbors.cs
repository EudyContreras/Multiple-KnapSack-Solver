using KnapSacks.Capsules;
using KnapSacks.Enumarations;
using KnapSacks.Interfaces;
using KnapSacks.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PriorityQueue.Collections;

namespace KnapSacks.Solvers
{
    public class SolverNeighbors : Solver
    {
        public static Solver Instance { get; private set; } = GetInstance();

        private static SolverNeighbors GetInstance()
        {
            return new SolverNeighbors();
        }

        public override Result Solve(DistributionType distribution, List<Bag> bags, List<Item> items)
        {
            switch (distribution)
            {
                case DistributionType.WeightDistributed:
                    return SolveByWeightDistribution(bags, items);
                case DistributionType.ValueDistributed:
                    return SolveByValueDistribution(bags, items);
                case DistributionType.NotDistributed:
                    return SolveWithNotDistribution(bags, items);
                default:
                    return SolveByWeightDistribution(bags, items);
            }
        }

        private Result SolveWithNotDistribution(List<Bag> bags, List<Item> items)
        {
            var itemsInBags = new List<Item>();

            var itemsNotAdded = new List<Item>();

            var accumulatedWeight = 0d;
            var accumulatedValue = 0d;

            foreach (var bag in bags.OrderBy(b => b.RemainingCapacity))
            {
                items = items.Where(item => !item.Included).OrderByDescending(item => item.Ratio).ToList();

                var queue = new PriorityQueue<Node>(Node.Comparer);

                var best = new Node();
                var root = new Node();

                root.ComputeBound(items, bag);

                queue.Offer(root);

                while (queue.Count >= 0)
                {
                    var node = queue.Count > 0 ? queue.Poll() : null;

                    if (node == null) break;

                    if ((node.valueBound > best.Value) && (node.Index < items.Count - 1))
                    {
                        var with = new Node(node);
                        var item = items[node.Index];

                        with.Weight += item.Weight;

                        if (with.Weight <= bag.RemainingCapacity)
                        {
                            with.Included.Add(items[node.Index]);
                            with.Value += item.Value;

                            with.ComputeBound(items, bag);

                            if (with.Value > best.Value)
                            {
                                best = with;
                            }

                            if (with.valueBound > best.Value)
                            {
                                queue.Offer(with);
                            }
                        }

                        var without = new Node(node);

                        without.ComputeBound(items, bag);

                        if (without.valueBound > best.Value)
                        {
                            queue.Offer(without);
                        }
                    }
                }

                foreach (var item in best.Included)
                {
                    items.Where(i => i.Id == item.Id).FirstOrDefault().Included = true;

                    accumulatedValue += item.Value;
                    accumulatedWeight += item.Weight;
                }

                bag.AddItems(best.Included.ToArray());
                itemsInBags.AddRange(best.Included);
            }

            foreach (var item in items)
            {
                if (itemsInBags.Find(i => i.Id == item.Id) == null)
                {
                    itemsNotAdded.Add(item);
                }
            }

            return new Result(bags, itemsNotAdded, accumulatedValue, accumulatedWeight);
        }

        private Result SolveByValueDistribution(List<Bag> bags, List<Item> items)
        {
            throw new NotImplementedException();
        }

        private Result SolveByWeightDistribution(List<Bag> bags, List<Item> items)
        {
            throw new NotImplementedException();
        }
    }
}

