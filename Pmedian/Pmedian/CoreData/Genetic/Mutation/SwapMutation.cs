using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Mutation
{
    class SwapMutation : AbstractMutation
    {
        public SwapMutation(double probability, int point) : base(probability, point) { }

        public override void Mutation(Chromosome chromosome)
        {
            if (Utility.Rand.NextDouble() < Probability)
            {
                for (int i = 0; i < Point; i++)
                {
                    int source = Utility.Rand.Next(chromosome.SizeChromosome);
                    int target = Utility.Rand.Next(chromosome.SizeChromosome);
                    Utility.Swap<int>(ref chromosome.chromosomeArray[source], ref chromosome.chromosomeArray[target]);
                } 
            }
        }

        public override void Mutation(List<Chromosome> childChromodome)
        {
            foreach (var ch in childChromodome)
                Mutation(ch);
        }
    }
}
