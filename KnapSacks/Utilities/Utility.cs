using System;
using System.Collections.Generic;
using System.Linq;
using KnapSacks.Capsules;
using KnapSacks.Models;
using System.IO;

namespace KnapSacks.Utilities
{
    internal class Utility
    {
        public const double MaxItemValue = 250;
        public const double MinItemValue = 1;

        public const double MaxItemWeight = 80;
        public const double MinItemWeight = 1;

        public const double MaxBagWeightCapacity = 360;
        public const double MinBagWeightCapacity = 50;

        internal static void PrintResults(Result result, List<Item> items, bool showLeftOverItems)
        {
            var bags = result.ResultingBags;

            var bagIndex = 0;

            var itemsInBags = new List<Item>();

            var itemsNotAdded = new List<Item>();

            Console.WriteLine($"Total Weight Capacity: {GetTotalCapacity(bags)}");
            Console.WriteLine($"Total Value : {result.TotalCost}");
            Console.WriteLine($"Total Weight : {result.TotalWeight}");
            Console.WriteLine($"Total Ratio : {result.ValueRatio}");
            Console.WriteLine($"Item Count: {result.TotalAddedItems}");

            foreach (var bag in bags)
            {
                Console.WriteLine();
                Console.WriteLine($"Bag {bagIndex }");
                Console.WriteLine($"Weight Capacity: {bag.WeightCapacity}");
                Console.WriteLine($"Reached Weight: {bag.AccumulatedWeight}");
                Console.WriteLine($"Reached Value: {bag.AccumulatedValue}");
                Console.WriteLine($"Item Count: {bag.ItemCount}");

                foreach(var item in bag.Items)
                {
                    Console.WriteLine($"    Item {item.Id } Weight: {item.Weight} Value: {item.Value} Ratio: {item.Ratio}");

                    itemsInBags.Add(item);
                }

                bagIndex++;
            }

            foreach (var item in items)
            {
                if (itemsInBags.Find(i => i.Id == item.Id) == null)
                {
                    itemsNotAdded.Add(item);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Items not added: {itemsNotAdded.Count}");
            Console.WriteLine($"Total value not added: {GetTotalValue(itemsNotAdded)}");
            Console.WriteLine($"Total weight not added: {GetTotalWeight(itemsNotAdded)}");

            if (!showLeftOverItems)
            {
                return;
            }

            foreach(var item in itemsNotAdded)
            {
                Console.WriteLine();
                Console.WriteLine($"    Item {item.Id }");
                Console.WriteLine($"        Weight: {item.Weight}");
                Console.WriteLine($"        Value: {item.Value}");
                Console.WriteLine($"        Ratio: {item.Ratio}");
            }            
        }

        public static int GetItemCount(List<Bag> bags) => bags.SelectMany(bag => bag.Items).Count();

        public static List<Bag> ReadBagsFile(string path)
        {
            var bags = new List<Bag>();

            var lines = File.ReadAllLines(path);

            foreach(string line in lines)
            {
                var parts = line.Split('|');

                var id = int.Parse(parts[0]);

                var Capacity = double.Parse(parts[1]);

                var bag = new Bag(id, Capacity);

                bags.Add(bag);
            }

            return bags;
        }

        public static void GenerateBagsFile(string path, double maxWeight, double minWeight, int bagCount)
        {
            var items = GenerateBags(maxWeight, minWeight, bagCount).Select(item => item.ToFileString());

            File.WriteAllLines(path, items);
        }

        public static List<Bag> GenerateBags(int bagCount) => GenerateBags(MaxBagWeightCapacity, MinBagWeightCapacity, bagCount);

        public static List<Bag> GenerateBags(double maxWeight, double minWeight, int bagCount)
        {
            var random = new Random();

            var bags = new List<Bag>();

            for(var i = 0; i<bagCount; i++)
            {
                var weightCapacity = GetRandomValue(random, maxWeight, minWeight);

                var bag = new Bag(i, weightCapacity);

                bags.Add(bag);
            }

            return bags;
        }

        public static List<Item> ReadItemsFile(string path)
        {
            var items = new List<Item>();

            var lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                var parts = line.Split('|');

                var id = int.Parse(parts[0]);

                var weight = double.Parse(parts[1]);

                var value = double.Parse(parts[2]);

                var item = new Item(id, weight, value);

                items.Add(item);
            }

            return items;
        }

        public static void GenerateItemsFile(string path, double maxWeight, double minWeight, double maxValue, double minValue, int itemCount)
        {
            var items = GenerateItems(maxWeight, minWeight, maxValue, minValue, itemCount).Select(item => item.ToFileString());

            File.WriteAllLines(path, items);
        }

        public static List<Item> GenerateItems(int itemCount) => GenerateItems(MaxItemWeight, MinItemWeight, MaxItemValue, MinItemValue, itemCount);

        public static List<Item> GenerateItems(double maxWeight, double minWeight, double maxValue, double minValue, int itemCount)
        {
            var random = new Random();

            var items = new List<Item>();

            for (var i = 0; i < itemCount; i++)
            {
                var weight = GetRandomValue(random, maxWeight, minWeight);
                var value = GetRandomValue(random, maxValue, minValue);

                var item = new Item(i, weight, value);

                items.Add(item);
            }

            return items;
        }

        public static double GetRandomValue(Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static bool ContainsItem(List<Bag> bags, Item item) => bags.Any(bag => bag.Items.Any(i => i.Id == item.Id));

        public static double GetTotalValue(List<Item> items) => items.Select(item => item.Value).Sum(item => item);

        public static double GetTotalWeight(List<Item> items) => items.Select(item => item.Weight).Sum(item => item);

        public static double GetTotalCapacity(List<Bag> bags) => bags.Select(bag => bag.WeightCapacity).Sum(bag => bag);
    }
}