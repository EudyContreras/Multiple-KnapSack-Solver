using KnapSacks.Capsules;
using KnapSacks.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using KnapSacks.Enumarations;
using KnapSacks.Models;
using KnapSacks.Utilities;

namespace KnapSacks.Solvers
{
    public class SolverGreedy : Solver
    {

        public static Solver Instance { get; private set; } = GetInstance();

        private static SolverGreedy GetInstance()
        {
            return new SolverGreedy();
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

            var bagCount = bags.Count;

            var totalCost = 0d;

            var totalWeight = 0d;

            foreach (var item in items.OrderByDescending(item => item.Ratio).ToList())
            {
                foreach (var bag in bags.OrderBy(bag => bag.RemainingCapacity).ToList()){

                    if (!bag.IsAtMaxCapacity)
                    {
                        if (bag.RemainingCapacity >= item.Weight && !Utility.ContainsItem(bags, item))
                        {
                            bag.AddItem(item);

                            itemsInBags.Add(item);

                            totalCost = totalCost + item.Value;
                            totalWeight = totalWeight + item.Weight;
                        }
                    }
                }
            }

            foreach (var item in items)
            {
                if (itemsInBags.Find(i => i.Id == item.Id) == null)
                {
                    itemsNotAdded.Add(item);
                }
            }

            return new Result(bags, itemsNotAdded, totalCost, totalWeight);
        }

        private static int TryOtherBags(List<Bag> bags, Bag currentBag, int bagIndex, int bagCount, Item item, ref double totalCost, ref double totalWeight)
        {
            for (var i = 0; i < bagCount; i++)
            {
                if (bags[i].Equals(currentBag) || bags[i].IsAtMaxCapacity) continue;

                if (bags[i].RemainingCapacity >= item.Weight && !Utility.ContainsItem(bags, item))
                {
                    bags[i].AddItem(item);
                   
                    totalCost = totalCost + item.Value;
                    totalWeight = totalWeight + item.Weight;
                }
            }

            if (bagIndex < bagCount - 1)
            {
                bagIndex++;
            }
            else
            {
                bagIndex = 0;
            }

            return bagIndex;
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
