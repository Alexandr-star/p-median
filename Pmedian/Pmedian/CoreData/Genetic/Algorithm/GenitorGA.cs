using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Selection;
using Pmedian.Model;
using Pmedian.Model.Enums;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Navigation;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class GenitorGA : AbstractGeneticAlgorithm
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
        /// Вариант кроссовера. 
        /// </summary>
        private CrossoverMethod crossoverMethod;

        /// <summary>
        /// Вероятность кроссовера.
        /// </summary>
        private double CrossoverProbability;

        /// <summary>
        /// Точек в кроссовере.
        /// </summary>
        private int dotCrossover;

        /// <summary>
        /// Вариант мутации.
        /// </summary>
        private MutationMethod mutationMethod;

        /// <summary>
        /// Вероятность мутации.
        /// </summary>
        private double MutationProbability;

        /// <summary>
        /// Точек в мутации.
        /// </summary>
        private int dotMutation;

        /// <summary>
        /// Минимальное хеминговое расстояние.
        /// </summary>
        private int minHemmingDistance;

        /// <summary>
        /// Конструктор с праметрами.
        /// </summary>
        /// <param name="IterationSize">Количество итераций в ГА.</param>
        /// <param name="PopulationSize">Размер популяции.</param>
        /// <param name="crossoverMethod">Оператор кроссовера.</param>
        /// <param name="CrossoverProbability">Вероятность кроссовера.</param>
        /// <param name="mutationMethod">Оператор мутации.</param>
        /// <param name="MutationProbability">Вероятность мутации.</param>
        public GenitorGA(int IterationSize, int PopulationSize,
            CrossoverMethod crossoverMethod, double CrossoverProbability, int dotCrossover,
            MutationMethod mutationMethod, double MutationProbability, int dotMutation,
            int minHemmingDistance)
            : base(IterationSize, PopulationSize)
        {
            this.crossoverMethod = crossoverMethod;
            this.CrossoverProbability = CrossoverProbability;
            this.mutationMethod = mutationMethod;
            this.MutationProbability = MutationProbability;
            this.dotCrossover = dotCrossover;
            this.dotMutation = dotMutation;
            this.minHemmingDistance = minHemmingDistance;
        }

        /// <summary>
        /// Реализация генетического алгоритма "Genitor".
        /// </summary>
        /// <param name="graph">Граф.</param>
        /// <param name="problemData">Параметры задачи</param>
        public override void GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
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
            //startPopulation.PrintPopulation();

            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, CrossoverProbability, dotCrossover, minHemmingDistance);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability, dotMutation);

            var population = startPopulation;
            Chromosome bestChromosome = null;
            double MediumFitness = 0;

            int stepGA = 0;
            bool answer = false;
            int countS = 0;
            while (stepGA <= IterateSize)
            {

                // вычесление пригодности хромосом.
                //Console.WriteLine("fitness ");
                for (int i = 0; i < PopulationSize; i++)
                {
                    population.populationList[i].fitness = Fitness.Function(cost, problemData, population.populationList[i]);
                    //Console.WriteLine(population.populationList[i].fitness);
                }

                MediumFitness = Solution.MediumFitnessPopulation(population);

                // вычесление ранга хромосомы.
                population.Sort();
                //population.PrintPopulation();
                /*for (int i = 0; i < PopulationSize; i++)
                {
                    Ranking(population.populationList[i], population.SizePopulation, i);
                    //Console.WriteLine($"{i} - {population.populationList[i].rank}");
                }*/
                //Console.WriteLine();

                // выбор двух хромосом для скрещивания
                List<Chromosome> selectedChromosome = RandomSelection.Selection(population.populationList);
                // полученный потомок после скрещивания
                //Console.WriteLine("crosss");

                Chromosome child = crossover.Crossover(selectedChromosome[0], selectedChromosome[1]);
                /*foreach (var ch in child.chromosomeArray)
                {
                    Console.Write(ch);                    
                }
                Console.WriteLine();*/

                if (mutation != null)
                {
                    mutation.Mutation(child);
                }


                // пригодность потомка
                child.fitness = Fitness.Function(cost, problemData, child);
                // поиск самой худщей хромосомы
                Chromosome badChrom = population.MinRankChromosome();
                //Console.WriteLine($"min rank{badChrom.rank}");
                // замена худшей хромосомы на потомка
                
                int index = population.populationList.IndexOf(badChrom);
                population.populationList.Insert(index, child);
                population.populationList.RemoveAt(index + 1);
                double tempMediumFitness = Solution.MediumFitnessPopulation(population);

                double absFitness = Math.Abs(tempMediumFitness - MediumFitness);
                

                stepGA++;

            }
            
            Console.WriteLine($"answer, step {stepGA}");
            population.PrintPopulation();
            
            if (!answer)
            {
                Console.WriteLine("Null best");
                bestChromosome = population.BestChromosome();
            }

            Console.WriteLine(bestChromosome.fitness);
            foreach (int ch in bestChromosome.chromosomeArray)
                Console.Write(ch);
            Console.WriteLine();
            AdjacencyList.GenerateList(bestChromosome, cost);

        }

        /// <summary>
        /// Вычесление ранга хромосомы.
        /// </summary>
        /// <param name="chromosome">Хромосома.</param>
        /// <param name="selectionPressure">Селективное давение.</param>
        /// <param name="sizePopulation">Размер популяции.</param>
        /// <param name="indexCh">Индекс хромосомы.</param>
        private void Ranking(Chromosome chromosome, int sizePopulation, int indexCh)
        {
            double a = Math.Round(Utility.Rand.NextDouble() + 1.0, 3);
            double b = Math.Round(2 - a, 3);
            double c = Math.Round(1.0 / sizePopulation, 3);
            double d = (double)indexCh / (double)(sizePopulation - 1);
            double f = Math.Round(a - (a - b));
            chromosome.rank = Math.Round(c * (f * (d)), 3);               
        }
             
    }
}
