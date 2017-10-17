using KnapSacks.Enumarations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnapSacks.Interfaces
{
    public interface ISolver<Result, Bags, Items>
    {
        Result Solve(DistributionType distribution, Bags output, Items input);
    }
}
