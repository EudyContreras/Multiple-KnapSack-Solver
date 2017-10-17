using KnapSacks.Capsules;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapSacks.Models
{
    public class Result
    {
        public List<Bag> ResultingBags { get; private set; }

        public List<Item> LeftOverItems { get; set; }

        public double TotalCost { get; private set; }

        public double TotalWeight { get; private set; }

        public double ValueRatio { get; private set; }

        public int TotalAddedItems { get; private set; }

        public Result(List<Bag> bags, List<Item> itemsNotAdded, double totalCost, double totalWeight)
        {
            this.ResultingBags = bags;
            this.TotalCost = totalCost;
            this.TotalWeight = totalWeight;
            this.ValueRatio = totalCost / totalWeight;
            this.LeftOverItems = itemsNotAdded;
            this.TotalAddedItems = Utilities.Utility.GetItemCount(bags);
        }
     }
}
