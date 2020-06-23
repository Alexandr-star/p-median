using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Selection
{
    public abstract class AbstractSelection : ISelection
    {
        public int countSelected;

        public int[] indexSelectChrom { get; private set; }

        public AbstractSelection(int countSelected)
        {
            this.countSelected = countSelected;
            this.indexSelectChrom = new int[countSelected];
        }

        public  abstract List<Chromosome> Selection(Population population);
        
    }
}
