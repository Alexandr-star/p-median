using Pmedian.CoreData.DataStruct;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

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
        public List<Chromosome> populationList { get; set; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="sizePopulation">Размер популяции.</param>
        /// <param name="costArray">Таблица расходов.</param>
        public Population(int sizePopulation, Cost costArray)
        {
            this.SizePopulation = sizePopulation;
            initializePopulation(costArray);
        }

        public Population(List<Chromosome> pop)
        {
            this.SizePopulation = pop.Count;
            this.populationList = pop;
        }

        private void initializePopulation(Cost cost)
        {
            this.populationList = new List<Chromosome>();
            int sizeChromosome = cost.countVillage * (cost.countClinic + cost.countMedic);
            for (int i = 0; i < SizePopulation; i++)
            {
                Chromosome chromosome = Chromosome.CreateChromosome(sizeChromosome);
                populationList.Add(chromosome);
            }
        }

        public void PrintPopulation()
        {
            Console.WriteLine($"Population - {SizePopulation}");
            for (int i = 0; i < populationList.Count; i++)
            {
                for (int j = 0; j < populationList[i].chromosomeArray.Length; j++)
                {
                    Console.Write($"{populationList[i].chromosomeArray[j]}");
                }
                Console.WriteLine();
            }
        }

        public Chromosome BestChromosome()
        {
            var bestChromosome = populationList[0];

            foreach( var chromosome in populationList)
            {
                if (chromosome.fitness < bestChromosome.fitness)
                    bestChromosome = chromosome;
            }

            return bestChromosome;
        }

        public Chromosome WorstChromosome()
        {
            var worstChromosome = populationList[0];

            foreach (var chromosome in populationList)
            {
                if (chromosome.fitness > worstChromosome.fitness)
                    worstChromosome = chromosome;
            }

            return worstChromosome;
        }
    }
}
