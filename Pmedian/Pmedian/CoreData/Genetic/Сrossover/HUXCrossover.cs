﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Сrossover
{
    /// <summary>
    /// Равномерный кроссовер, при котором только половина битов каждой хромосомы переходит к потомку.
    /// </summary>
    class HUXCrossover : AbstractCrossover
    {
        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="probability">Вероятность кроссовера.</param>
        public HUXCrossover(double probability) : base(probability)
        {

        }

        /// <summary>
        /// Реализация кроссовера, при котором на вход подается список родителей.
        /// А на выходе список потомков.
        /// </summary>
        /// <param name="parents">Список родителей.</param>
        /// <returns>Список потомков</returns>
        public override List<Chromosome> Crossover(List<Chromosome> parents)
        {
            if (Probability == 0)
                throw new NotImplementedException();

            List<Chromosome> childrenList = new List<Chromosome>();
            double probability = Utility.Rand.NextDouble();
        
            if (probability <= Probability)
            {
                int[] indexes = Utility.ShuffleIndexes(parents.Count);
                for (int p = 0; p < parents.Count; p += 2)
                {
                    int[] firstChild = parents.ElementAt(indexes[p]).chromosomeArray;
                    int[] secondChild = parents.ElementAt(indexes[p + 1]).chromosomeArray;
                    
                    List<int> res = H(firstChild, secondChild, firstChild.Length);
                    foreach (int i in res)
                        Console.Write(i);
                    Console.WriteLine();

                    Console.WriteLine($"{HemmingDistance(firstChild, secondChild)}");
                    int halfDistance = HemmingDistance(firstChild, secondChild) / 2;

                    for (int i = 0; i < halfDistance; i++)
                    {
                        int indexBitInList = Utility.Rand.Next(res.Count);
                        int indexBit = res.ElementAt(indexBitInList);

                        Utility.Swap<int>(ref firstChild[indexBit], ref secondChild[indexBit]);
                        res.RemoveAt(indexBitInList);
                    }

                    childrenList.Add(new Chromosome(firstChild));
                    childrenList.Add(new Chromosome(secondChild));
                }
            }

            return childrenList;
        }

        /// <summary>
        /// Реализация кроссовера, при котором на вход два родителя.
        /// А на выходе один потомок.
        /// </summary>
        /// <param name="firstParent">Первый родитель.</param>
        /// <param name="secondParent">Второй родитель.</param>
        /// <returns>Один потомок.</returns>
        public override Chromosome Crossover(Chromosome firstParent, Chromosome secondParent)
        {
            if (Probability == 0)
                throw new NotImplementedException();

            double probability = Utility.Rand.NextDouble();

            int sizeChromosome = firstParent.SizeChromosome;
            int[] firstChild = firstParent.chromosomeArray;
            int[] secondChild = secondParent.chromosomeArray;

            firstParent.PrintChromosome();
            secondParent.PrintChromosome();

            

            if (probability <= Probability)
            {
                List<int> res = H(firstParent.chromosomeArray, secondParent.chromosomeArray, sizeChromosome);
                foreach (int i in res)
                    Console.Write(i);
                Console.WriteLine();

                Console.WriteLine($"{HemmingDistance(firstParent.chromosomeArray, secondParent.chromosomeArray)}");
                int halfDistance = HemmingDistance(firstParent.chromosomeArray, secondParent.chromosomeArray) / 2;
               

                for (int i = 0; i < halfDistance; i++)
                {
                    int indexBitInList = Utility.Rand.Next(res.Count);
                    int indexBit = res.ElementAt(indexBitInList);

                    Utility.Swap<int>(ref firstChild[indexBit], ref secondChild[indexBit]);
                    res.RemoveAt(indexBitInList);
                }
            }

            Chromosome child = new Chromosome(firstChild);
            Chromosome c = new Chromosome(secondChild);
            child.PrintChromosome();
            c.PrintChromosome();
            return child;
        }

        private int HemmingDistance(int[] first, int[] second)
        {
            int distance = first.Zip(second, (f, s) => new { f, s }).Count(m => m.f != m.s);

            return distance;
        }

        private List<int> H(int[] first, int[] second, int size)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < size; i++)
            {
                if (first[i] != second[i])
                    res.Add(i);
            }

            return res;
        }
    }
}
