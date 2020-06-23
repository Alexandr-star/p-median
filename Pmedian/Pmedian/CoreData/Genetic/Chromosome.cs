using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic
{
    /// <summary>
    /// Класс характеризующий хромосому (потомка).
    /// </summary>
    public class Chromosome
    {
        /// <summary>
        /// Приспособленность хромосомы.
        /// </summary>
        public double fitness { get; set; }
       
        /// <summary>
        /// Ранг хромосомы.
        /// </summary>
        public double rank { get; set; }
       

        /// <summary>
        /// Длинна хромосомы.
        /// </summary>
        private int _sizeChromosome => chromosomeArray.Length;

        public int SizeChromosome { get => _sizeChromosome; }
        /// <summary>
        /// Массив описывающий хромосому.
        /// </summary>
        public int[] chromosomeArray { get;  set; }

        public Chromosome(int[] chromosome)
        {
            this.chromosomeArray = chromosome;
        }

        public static Chromosome CreateChromosome(int sizeChromosome)
        {           
            int[] chromosomeArray = new int[sizeChromosome];
            for (int i = 0; i < sizeChromosome; i++)
            {
                double p = Utility.Rand.NextDouble();
                if (p < 0.5)
                    chromosomeArray[i] = 0;
                else
                    chromosomeArray[i] = 1;
            }
            Chromosome chromosome = new Chromosome(chromosomeArray);

            return chromosome;
        }        
    }
}
