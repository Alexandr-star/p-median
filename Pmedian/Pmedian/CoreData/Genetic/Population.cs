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

        /// <summary>
        /// Размер хромосомы.
        /// </summary>
        public int SizeChromosome { get; }

        /// <summary>
        /// Популяция, предаставленная массивом хромомсом.
        /// </summary>
        public List<Chromosome> populationList { get; set; }

        /// <summary>
        /// Наименьший ранк хромосомы.
        /// </summary>
        private double MinRankChromosome { get; set; }

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
            //int sizeChromosome = cost.countClinic + cost.countMedic;
            //int sizeChromosome = 0;
            //foreach (var list in cost.CostList)
            //{
            //    sizeChromosome += list.Count;
            //}
            for (int i = 0; i < SizePopulation; i++)
            {
                Chromosome chromosome = Chromosome.CreateChromosome(cost.vertexCount - cost.countVillage);
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
                Console.WriteLine($"rank {populationList[i].rank}");
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

        public Chromosome ChromosomeWithMinRank()
        {
            Chromosome minRankChromosome = populationList[0];

            foreach (var chromosome in populationList)
            {
                if (chromosome.rank < minRankChromosome.rank)
                    minRankChromosome = chromosome;
            }
            MinRankChromosome = minRankChromosome.rank;
            return minRankChromosome;
        }


        public Chromosome OneOfChromosomesWithMinRank()
        {
           
            List<Chromosome> list = new List<Chromosome>();
            MinRankChromosome = populationList[SizePopulation - 1].rank;
            foreach (var chromosome in populationList)
            {
                if (chromosome.rank < MinRankChromosome)
                    MinRankChromosome = chromosome.rank;
            }
            Chromosome ch = populationList[0];
            try
            {
                foreach (var chromosome in populationList)
                {
                    if (chromosome.rank == MinRankChromosome)
                        list.Add(chromosome);
                    else
                        break;
                }

                int index = Utility.Rand.Next(list.Count);
                ch = list[index];


            } catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Ex");
                Console.WriteLine($"data {ex.Data}\n" +
                    $"actual value {ex.ActualValue}\n" +
                   $"param name {ex.ParamName}\n" +
                    $"source {ex.Source}\n" +
                    $"stack trace{ex.StackTrace}");
            }
            return ch;
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
                    if (temp > min)
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
