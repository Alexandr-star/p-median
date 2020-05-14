using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Сrossover
{
    class NDotCrossover : AbstractCrossover
    {
        private int CountDot;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="probability"></param>
        public NDotCrossover(int countDot, double probability) : base(probability)
        {
            this.CountDot = countDot;
        }

        public override List<int[]> Crossover(List<int[]> parents)
        {
            if (Probability == 0)
                throw new NotImplementedException();

            List<int[]> childrenList = new List<int[]>();
            double probability = Utility.Rand.NextDouble();
            if (probability <= Probability)
            {
                int[] indexes = Utility.ShuffleIndexes(parents.Count);
                for (int p = 0; p < parents.Count; p += 2)
                {
                    int[] firstParent = parents.ElementAt(indexes[p]);
                    int[] secondParent = parents.ElementAt(indexes[p + 1]);
                    int sizeChromosome = firstParent.Length;
                    int[] dots = GetDotCrossover(CountDot, sizeChromosome);
                    Console.WriteLine("dots");
                    foreach (int i in dots)
                        Console.Write(i);
                    Console.WriteLine("dots");

                    int[] firstChild = new int[sizeChromosome];
                    int[] secondChild = new int[sizeChromosome];

                    int g = 0;
                    for (int d = 0; d < CountDot; d++)
                    {
                        for (int i = g; i < sizeChromosome; i++)
                        {
                            if (d % 2 == 0 || d == 0)
                            {
                                if (i < dots[d])
                                {
                                    firstChild[i] = firstParent[i];
                                    secondChild[i] = secondParent[i];
                                    g++;
                                }
                                else if (p == CountDot - 1)
                                {
                                    firstChild[i] = secondParent[i];
                                    secondChild[i] = firstParent[i];
                                }
                                else
                                    break;
                            }
                            else
                            {
                                if (i < dots[d])
                                {
                                    firstChild[i] = secondParent[i];
                                    secondChild[i] = firstParent[i];
                                    g++;
                                }
                                else if (p == CountDot - 1)
                                {
                                    firstChild[i] = firstParent[i];
                                    secondChild[i] = secondParent[i];
                                }
                                else
                                    break;
                            }
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
                int[] dots = GetDotCrossover(CountDot, sizeChromosome);
                Console.WriteLine("dots");
                foreach (int i in dots)
                    Console.Write(i);
                Console.WriteLine("dots");

                int g = 0;
                for (int p = 0; p < CountDot; p++)
                {
                    for (int i = g; i < sizeChromosome; i++)
                    {
                        if (p % 2 == 0 || p == 0)
                        {
                            if (i < dots[p])
                            {
                                childArray[i] = firstParent.chromosomeArray[i];
                                g++;
                            } else if (p == CountDot - 1)
                            {
                                childArray[i] = secondParent.chromosomeArray[i];
                            }
                            else
                                break;
                        } else
                        {
                            if (i < dots[p])
                            {
                                childArray[i] = secondParent.chromosomeArray[i];
                                g++;
                            } else if (p == CountDot - 1)
                            {
                                childArray[i] = firstParent.chromosomeArray[i];
                            }
                            else
                                break;
                        }
                    }
                }
            }
            Chromosome child = new Chromosome(childArray);
            return child;
        }

        /// <summary>
        /// Возвращает массив с точками кроссовера, сгенерированными случайно.
        /// </summary>
        /// <param name="countDot">Количество точек.</param>
        /// <returns>Массив точек.</returns>
        private int[] GetDotCrossover(int countDot, int sizeChromosome)
        {
            int[] dots = new int[countDot];
            List<int> dotslist = new List<int>(countDot);
            dotslist.Add(Utility.Rand.Next(sizeChromosome));
            int dot = 0;
            for (int i = 0; i < countDot; i++)
            {               
                do
                {
                    dot = Utility.Rand.Next(1, sizeChromosome);
                } while (dots.Contains(dot));
                dots[i] = dot;
            }

            Array.Sort(dots);
            return dots;
        }
    }
}
