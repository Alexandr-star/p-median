using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Сrossover;
using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class CHCGA : AbstractGeneticAlgorithm
    {
        /// <summary>
        /// Граф, задаваемый списком смежности вершин.
        /// </summary>
        private AdjacencyList adjacencyList;

        /// <summary>
        /// Таблица расходов.
        /// </summary>
        private Cost cost;
       
        /// <summary>
        /// Вероятность кроссовера.
        /// </summary>
        private double CrossoverProbability;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="IterationSize">Количество итераций ГА.</param>
        /// <param name="PopulationSize">Размер популяции.</param>
        /// <param name="crossoverMethod">Оператор кроссовера.</param>
        /// <param name="CrossoverProbability">Вероятность кроссовера.</param>
        public CHCGA(int IterationSize, int PopulationSize,
            double CrossoverProbability)
            : base(IterationSize, PopulationSize)
        {
            this.CrossoverProbability = CrossoverProbability;           
        }

        /// <summary>
        /// Реализация генетического алгоритма "CHC".
        /// </summary>
        /// <param name="graph">Граф.</param>
        /// <param name="problemData">Параметры задачи.</param>
        public override void GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            Console.WriteLine("CHC");
            // Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.CreateCostArray(graph);
            ProblemData problem = problemData;
            Console.WriteLine("Problem");
            Console.WriteLine(problem.P);
            Console.WriteLine(problem.RoadCost);
            Console.WriteLine(problem.TimeAmbulance);
            Console.WriteLine(problem.TimeMedic);
            Population startPopulation = new Population(PopulationSize, cost);


            HUXCrossover crossover = new HUXCrossover(CrossoverProbability);

            var population = startPopulation;

            int stepGA = 1;

            while (stepGA <= IterateSize)
            {
                // Вычесление пригодности хромосом.
                for (int i = 0; i < PopulationSize; i++)
                {
                    population.populationList[i].fitness = Fitness.Function(cost, problemData, population.populationList[i]);
                    //Console.WriteLine(population.populationList[i].fitness);
                }
                
                List<Chromosome> child = crossover.Crossover(population.populationList);
                
                foreach (var ch in child)
                {
                    foreach (int gen in ch.chromosomeArray)
                    {
                        Console.Write(gen);
                    }
                    Console.WriteLine();
                }

                stepGA++;
            }
        }
    }
}
