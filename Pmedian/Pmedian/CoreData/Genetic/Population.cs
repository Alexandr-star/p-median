using Pmedian.CoreData.DataStruct;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic
{
    /// <summary>
    /// Класс реализующий популяцию хромосом.
    /// </summary>
    class Population
    {
        /// <summary>
        /// Размер популяции.
        /// </summary>
        public int SizePopulation { get; set; }

        public int SizeChromosome { get; }

        /// <summary>
        /// Популяция, предаставленная массивом хромомсом.
        /// </summary>
        public List<int[]> populationList { get; set; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="sizePopulation">Размер популяции.</param>
        /// <param name="costArray">Таблица расходов.</param>
        public Population(int sizePopulation, Cost costArray)
        {
            this.SizePopulation = sizePopulation;
            this.SizeChromosome = costArray.countVillage * costArray.countOtherPoint;
            initializePopulation(costArray);
        }

        public Population(List<int[]> pop)
        {
            this.SizePopulation = pop.Count;
            this.SizeChromosome = pop[0].Length;
            this.populationList = pop;
        }

        private void initializePopulation(Cost costArray)
        {
            this.populationList = new List<int[]>();

            for (int i = 0; i < SizePopulation; i++)
            {
                int[] chromosome = InitializeChromosome();
                populationList.Add(chromosome);
                Console.WriteLine();
                //Console.WriteLine($"{populationList[i].GetHashCode()}  {populationList[i].chromosomeArray.GetHashCode()}");

                //array[i].PrintChromosome();
            }
            PrintPopulation();
        }

        private int[] InitializeChromosome()
        {
            int[] chromosomeArray = new int[SizeChromosome];
            for (int i = 0; i < SizeChromosome; i++)
            {
                double p = Utility.Rand.NextDouble();
                if (p < 0.5)
                {
                    chromosomeArray[i] = 0;
                }
                else
                {
                    chromosomeArray[i] = 1;
                }

            }
            return chromosomeArray;
        }

        public void PrintPopulation()
        {
            Console.WriteLine($"Population - {SizePopulation}");
            for (int i = 0; i < populationList.Count; i++)
            {
                for (int j = 0; j < SizeChromosome; j++)
                {
                    Console.Write(populationList[i][j]);
                }
                Console.WriteLine();
            }
        }
    }
}
