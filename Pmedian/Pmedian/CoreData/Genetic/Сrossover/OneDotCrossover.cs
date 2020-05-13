using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Сrossover
{
    /// <summary>
    /// Реализация алгоритма одноточечного кроссовера.
    /// </summary>
    class OneDotCrossover : AbstractCrossover
    {   
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="probability"></param>
        public OneDotCrossover(double probability) : base(probability)
        {

        }

        /// <summary>
        /// Одноточечный кроссовер.
        /// </summary>
        /// <param name="parents">Список с родителями, который будут скещиваться</param>
        /// <returns>Список потомков.</returns>
        public override List<int[]> Crossover(List<int[]> parents)
        {
            if (Probability == 0)
                throw new NotImplementedException();

            List<int[]> childrenList = new List<int[]>();
            double probability = Utility.Rand.NextDouble();
            if (probability <= Probability)
            {
                int[] indexes = ShuffleIndexes(parents.Count);
                for (int i = 0; i < parents.Count; i += 2)
                {
                    int[] firstParent = parents.ElementAt(indexes[i]);
                    int[] secondParent = parents.ElementAt(indexes[i + 1]);
                    int sizeChromosome = firstParent.Length;

                    int pointCrossover = Utility.Rand.Next(sizeChromosome - 1) + 1;

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

                    childrenList.Add(firstChild);
                    childrenList.Add(secondChild);

                }
            }

            return childrenList;
        }

        public override Chromosome Crossover(Chromosome firstParent, Chromosome secondParent)
        {          
            if (Probability == 0)
                throw new NotImplementedException();
            
            double probability = Utility.Rand.NextDouble();
                        
            int sizeChromosome = firstParent.SizeChromosome;
            int[] childArray = new int[sizeChromosome];
            
            if (probability <= Probability)
            {                
                int pointCrossover = Utility.Rand.Next(sizeChromosome - 1) + 1;
                for (int p = 0; p < sizeChromosome; p++)
                {
                    if (p < pointCrossover)
                    {
                        childArray[p] = firstParent.chromosomeArray[p];
                    }
                    else
                    {
                        childArray[p] = secondParent.chromosomeArray[p];
                    }                    
                }
            }
            Chromosome child = new Chromosome(childArray);
            return child;
        }

        public override int[] ShuffleIndexes(int size)
        {
            int[] indexes = new int[size];
            for (int i = 0; i < size; i++)
                indexes[i] = i;

            int randomIndex = 0;
            for (int i = 0; i < size; i++)
            { 
                randomIndex = Utility.Rand.Next(size);
                Utility.Swap<int>(ref indexes[i], ref indexes[randomIndex]);
            }
            
            return indexes;
        }
    }
}
