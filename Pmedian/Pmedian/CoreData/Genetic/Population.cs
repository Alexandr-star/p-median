using Pmedian.CoreData.DataStruct;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class Population
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
            //int sizeChromosome = cost.countVillage * (cost.countClinic + cost.countMedic);
            int sizeChromosome = 0;
            foreach (var list in cost.CostList)
            {
                sizeChromosome += list.Count;
            }
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
                Console.WriteLine($"fit {populationList[i].fitness}");
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

        public Chromosome MinRankChromosome()
        {
            var minRankChromosome = populationList[0];

            foreach (var chromosome in populationList)
            {
                if (chromosome.rank < minRankChromosome.rank)
                    minRankChromosome = chromosome;
            }

            return minRankChromosome;
        }

        public void Sort()
        {
            for (int i = 0; i < SizePopulation - 1; i++)
            {
                double min = populationList[i].fitness;
                int minId = i;
                for (int j = i + 1; j < SizePopulation; j++)
                {
                    double temp = populationList[j].fitness;
                    if (temp < min)
                    {
                        min = temp;
                        minId = j;
                    }
                }

                Chromosome tempChromosome = populationList[i];
                populationList.Insert(i, populationList[minId]);
                populationList.RemoveAt(i + 1);
                populationList.Insert(minId, tempChromosome);
                populationList.RemoveAt(minId + 1);

            }
        }
    }
}
