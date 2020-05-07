using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Сrossover
{    
   abstract class AbstractCrossover : ICrossover
   {
        public double Probability { get; }

        public AbstractCrossover(double probability)
        {
            this.Probability = probability;
        }

        public abstract List<Chromosome> Crossover(List<Chromosome> parents);
        public abstract int[] ShuffleIndexes(int size, Random random);
   }   
}
