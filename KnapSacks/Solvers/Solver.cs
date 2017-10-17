using KnapSacks.Capsules;
using KnapSacks.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using KnapSacks.Enumarations;
using KnapSacks.Models;

namespace KnapSacks.Solvers
{
    public abstract class Solver : ISolver<Result, List<Bag>, List<Item>>
    {
        public abstract Result Solve(DistributionType distribution, List<Bag> output, List<Item> input);
    }
}
