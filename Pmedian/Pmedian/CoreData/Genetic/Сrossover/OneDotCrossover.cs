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
        public OneDotCrossover(double probability) : base(probability) { }

        /// <summary>
        /// Одноточечный кроссовер.
        /// </summary>
        /// <param name="parents">Список с родителями, который будут скещиваться</param>
        /// <returns>Список потомков.</returns>
        public override List<Chromosome> Crossover(List<Chromosome> parents)
        {
            if (Probability == 0)
                throw new NotImplementedException();

            List<Chromosome> childrenList = new List<Chromosome>();
            int[] indexes = Utility.ShuffleIndexes(parents.Count);
            for (int i = 0; i < parents.Count; i += 2)
            {
                                        
                double probability = Utility.Rand.NextDouble();
                if (parents.Count % 2 == 1 && i == parents.Count - 1)
                    probability = 1;
                if (probability <= Probability)
                {
                    int[] firstParent = parents.ElementAt(indexes[i]).chromosomeArray;
                    int[] secondParent = null;
                    if (parents.Count % 2 == 1 && i == parents.Count - 1)
                        secondParent = parents.ElementAt(indexes[0]).chromosomeArray;
                    else
                        secondParent = parents.ElementAt(indexes[i + 1]).chromosomeArray;
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
                        }
                        else
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
    }
}
