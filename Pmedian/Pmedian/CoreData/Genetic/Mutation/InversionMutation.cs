using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Mutation
{
    class InversionMutation : AbstractMutation
    {
        public InversionMutation(double probability, int point) : base(probability, point) { }       

        public override void Mutation(Chromosome chromosome)
        {
            if (Utility.Rand.NextDouble() < Probability)
            {
                int p = Utility.Rand.Next(chromosome.SizeChromosome - Point);
                for (int i = p, j = 0; i < p + Point - 1; i++, j++)
                {
                    if (i == p + Point - 1 - j) break;
                    Utility.Swap<int>(ref chromosome.chromosomeArray[i], ref chromosome.chromosomeArray[p + Point - 1 - j]);
                }
                
            }
        }

        public override void Mutation(List<Chromosome> childChromodome)
        {
            throw new NotImplementedException();
        }
    }
}
