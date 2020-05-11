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
        public ReplaceMutaion(double probabililt) : base(probabililt) { }

        /// <summary>
        /// Мутаия в одной точке.
        /// </summary>
        /// <param name="chromosome">Хромомсома</param>
        public override void Mutation(int[] chromosome)
        {
            int indexGen = Utility.Rand.Next(chromosome.Length);

            if (chromosome[indexGen] == 0)
                chromosome[indexGen] = 1;
            else
                chromosome[indexGen] = 0;
        }
    }
}
