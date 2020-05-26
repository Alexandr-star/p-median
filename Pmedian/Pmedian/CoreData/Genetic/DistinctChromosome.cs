using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Pmedian.CoreData.Genetic
{
    class DistinctChromosome : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(Item obj)
        {
            throw new NotImplementedException();
        }
    }
}
