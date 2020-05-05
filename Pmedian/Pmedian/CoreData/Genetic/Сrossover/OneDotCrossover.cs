using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Сrossover
{
    /// <summary>
    /// Реализация алгоритма одноточечного кроссовера.
    /// </summary>
    class OneDotCrossover : ICrossover
    {
        /// <summary>
        /// Вероятность кроссовера.
        /// </summary>
        private double PROBABILITY_CROSSOVER = 0;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public OneDotCrossover(double probabilityCrossover)
        {
            this.PROBABILITY_CROSSOVER = probabilityCrossover;
        }

        public List<Chromosome> Crossover(List<Chromosome> parents)
        {           
            if (PROBABILITY_CROSSOVER == 0)
                throw new NotImplementedException();

            List<Chromosome> childrenList = new List<Chromosome>();
            Random random = new Random();
            double probability = random.NextDouble();
            if (probability <= PROBABILITY_CROSSOVER)
            {
                int[] indexes = ShuffleIndexes(parents.Count, random);
                for (int i = 0; i < parents.Count; i += 2)
                {
                    int[] firstParent = parents.ElementAt(indexes[i]).GetArrayGensCromosome();
                    int[] secondParent = parents.ElementAt(indexes[i + 1]).GetArrayGensCromosome();
                    int sizeChromosome = firstParent.Length;

                    int pointCrossover = random.Next(sizeChromosome - 1) + 1;

                    int[] firstChild = new int[sizeChromosome];
                    int[] secondChild = new int[sizeChromosome];

                    for (int p = 0; p < sizeChromosome; p++)
                    {
                        if (p < pointCrossover)
                        {
                            firstChild[p] = firstParent[p];
                            secondChild[p] = secondParent[p];
                        } else
                        {
                            firstChild[p] = secondParent[p];
                            secondChild[p] = firstParent[p];
                        }
                    }

                    childrenList.Add(new Chromosome(firstChild));
                    childrenList.Add(new Chromosome(secondChild));

                }
            }

            return childrenList;
        }

        private int[] ShuffleIndexes(int size, Random random)
        {
            int[] indexes = new int[size];
            int randomIndex = 0;
            for (int i = 0; i < size; i++)
            {
                do
                {
                    randomIndex = random.Next(size);
                } while (Array.Exists(indexes, element => element == randomIndex));
                indexes[i] = randomIndex;
            }
            return indexes;
        }

        /// <summary>
        /// Одноточечный кроссовер.
        /// </summary>
        /// <param name="parents">Список с родителями, который будут скещиваться</param>
        /// <returns>Список потомков.</returns>



    }
}
