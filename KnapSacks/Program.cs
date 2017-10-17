using KnapSacks.Capsules;
using KnapSacks.Enumarations;
using KnapSacks.Models;
using KnapSacks.Solvers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using KnapSacks.Utilities;

namespace KnapSacks
{
    public class Program
    {
        public const string BagDataPath = @"../../../KnapSacks/TestData/BagData.txt";
        public const string ItemDataPath = @"../../../KnapSacks/TestData/ItemData.txt";

        public static bool autoGenerate = false;
        public static bool regenerateFile = false;

        public static void Main(string[] args)
        {
            var items = new List<Item>();

            var bags = new List<Bag>();

            var bagCount = 9;

            var itemCount = 160;

            if (!autoGenerate)
            {
                if (regenerateFile)
                {
                    Utility.GenerateItemsFile(ItemDataPath, Utility.MaxItemWeight, Utility.MinItemWeight, Utility.MaxItemValue, Utility.MinItemValue, itemCount);
                    Utility.GenerateBagsFile(BagDataPath, Utility.MaxBagWeightCapacity, Utility.MinBagWeightCapacity, bagCount);
                }
                else
                {
                    if (File.ReadAllLines(ItemDataPath).Count() <= 0)
                    {
                        Utility.GenerateItemsFile(ItemDataPath, Utility.MaxItemWeight, Utility.MinItemWeight, Utility.MaxItemValue, Utility.MinItemValue, itemCount);
                    }
                    if (File.ReadAllLines(BagDataPath).Count() <= 0)
                    {
                        Utility.GenerateBagsFile(BagDataPath, Utility.MaxBagWeightCapacity, Utility.MinBagWeightCapacity, bagCount);
                    }
                }

                items.AddRange(Utility.ReadItemsFile(ItemDataPath));

                bags.AddRange(Utility.ReadBagsFile(BagDataPath));
            }
            else
            {
                items.AddRange(Utility.GenerateItems(itemCount));

                bags.AddRange(Utility.GenerateBags(bagCount));
            }


            //var result = SolverGreedy.Instance.Solve(DistributionType.NotDistributed, bags, items);

            var result = SolverNeighbors.Instance.Solve(DistributionType.NotDistributed, bags, items);

            Utility.PrintResults(result, items, showLeftOverItems : false);

            Console.ReadLine();
        }
    }
}