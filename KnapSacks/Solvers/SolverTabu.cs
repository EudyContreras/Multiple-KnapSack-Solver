using KnapSacks.Capsules;
using KnapSacks.Enumarations;
using KnapSacks.Interfaces;
using KnapSacks.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace KnapSacks.Solvers
{
    public class SolverTabu : Solver
    {
        public static Solver Instance { get; private set; } = GetInstance();

        private static SolverTabu GetInstance()
        {
            return new SolverTabu();
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
            var result = SolverGreedy.Instance.Solve(DistributionType.NotDistributed, bags, items);

            foreach (Bag bag in result.ResultingBags)
            {
                PerformSwapps(result, bag, 1.3);
            }

            return result;
        }

        private void PerformSwapps(Result result, Bag currentBag, double thresholdDrift)
        {
            var searchedItems = new List<Item>();

            var accumulatedWeight = 0d;
            var accumulatedValue = 0d;

            var weightThreshold = (currentBag.RemainingCapacity + (result.LeftOverItems.OrderBy(i => i.Weight).FirstOrDefault().Weight) * thresholdDrift);

            foreach (Item item in result.LeftOverItems.OrderBy(i => i.Weight).ToList())
            {
                if (accumulatedWeight < weightThreshold)
                {
                    if (WeightToBeAdded(accumulatedWeight, item.Weight) <= weightThreshold)
                    {
                        accumulatedWeight += item.Weight;
                        accumulatedValue += item.Value;

                        searchedItems.Add(item);
                    }
                }
            }

            var itemToSwapp = GetNeighboringItem(NeighborType.WeightNeighbor, currentBag, accumulatedWeight, accumulatedValue);

            if (itemToSwapp != null)
            {
                currentBag.RemoveItem(itemToSwapp);

                currentBag.AddItems(searchedItems.ToArray());
            }
        }

        private double WeightToBeAdded(double accumulatedWeight, double itemWeight) => accumulatedWeight + itemWeight;

        private Item GetNeighboringItem(NeighborType type, Bag bag, double accumulatedWeight, double accumulatedValue)
        {
            if (type == NeighborType.WeightNeighbor)
            {
                var lowerLimit = accumulatedWeight * 0.65;

                var totalAllowence = (bag.WeightCapacity - bag.AccumulatedWeight) + lowerLimit;

                var fittingItem = bag.Items
                    .Where(item => item.Weight >= lowerLimit && item.Weight <= totalAllowence)
                    .Select(item => item)
                    .Where(item => item.Value < accumulatedValue)
                    .OrderBy(item => item.Value)
                    .FirstOrDefault();

                if (fittingItem != null)
                {
                    var replacement = fittingItem;
                }

                return fittingItem;
            }
            else
            {
                return null;
            }
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

