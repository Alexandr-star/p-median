using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Mutation
{
    class ReplaceMutaion : AbstractMutation
    {
        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        public ReplaceMutaion(double probabililt, int point) : base(probabililt, point) { }

        /// <summary>
        /// Мутаия в одной точке.
        /// </summary>
        /// <param name="chromosome">Хромомсома</param>
        public override void Mutation(Chromosome chromosome)
        {
            if (Utility.Rand.NextDouble() < Probability)
            {
                for (int i = 0; i < Point; i++)
                {
                    int indexGen = Utility.Rand.Next(chromosome.chromosomeArray.Length);

                    if (chromosome.chromosomeArray[indexGen] == 0)
                        chromosome.chromosomeArray[indexGen] = 1;
                    else
                        chromosome.chromosomeArray[indexGen] = 0;
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
