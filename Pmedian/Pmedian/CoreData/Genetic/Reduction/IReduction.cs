using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Reduction
{
    interface IReduction
    {
        void Reduction(List<Chromosome> intermidatePopulation);

        void Reduction(List<Chromosome> childPopulation, List<Chromosome> population);
    }
}
