using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Selection;
using Pmedian.Model;
using Pmedian.Model.Enums;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class GenitorGA : AbstractGeneticAlgorithm
    {
        /// <summary>
        /// Граф, задаваемый списком смежности вершин.
        /// </summary>
        private AdjacencyList adjacencyList;

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
        /// Вариант мутации.
        /// </summary>
        private MutationMethod mutationMethod;

        /// <summary>
        /// Вероятность мутации.
        /// </summary>
        private double MutationProbability;



        public GenitorGA(int IterationSize, int PopulationSize,
            CrossoverMethod crossoverMethod, double CrossoverProbability,
            MutationMethod mutationMethod, double MutationProbability)
            : base(IterationSize, PopulationSize)
        {
            this.crossoverMethod = crossoverMethod;
            this.CrossoverProbability = CrossoverProbability;
            this.mutationMethod = mutationMethod;
            this.MutationProbability = MutationProbability;

        }

        /// <summary>
        /// Реализация генетического алгоритма "Genitoe".
        /// </summary>
        /// <param name="graph">Граф.</param>
        /// <param name="problemData">Параметры задачи</param>
        public override void GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            //Инициализация основных структур.
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

            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, CrossoverProbability);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability);

            var population = startPopulation;

            int stepGA = 1;

            while (stepGA <= IterateSize)
            {

                // вычесление пригодности хромосом.
                //Console.WriteLine("fitness ");
                for (int i = 0; i < PopulationSize; i++)
                {
                    population.populationList[i].fitness = Fitness.Function(cost, problemData, population.populationList[i]);
                    //Console.WriteLine(population.populationList[i].fitness);
                }

                // вычесление селективного давления.
                double selectionPressure = SelectionPressure.S(population.WorstChromosome(), population);
                // вычесление ранга хромосомы.
                population.Sort();
                //population.PrintPopulation();
                for (int i = 0; i < PopulationSize; i++)
                {
                    double rank = Ranking(population.populationList[i], population.SizePopulation, i);
                    Console.WriteLine(population.populationList[i].rank);
                }
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
                // замена худшей хромосомы на потомка
                
                   int index = population.populationList.IndexOf(badChrom);
                   population.populationList.Insert(index, child);
                population.populationList.RemoveAt(index + 1);


                stepGA++;

            }
            //Console.WriteLine("answer");
            Chromosome winCh = population.BestChromosome();
            //Console.WriteLine(winCh.fitness);
            int indexWin = population.populationList.IndexOf(winCh);


            Chromosome win = population.populationList[indexWin];
            //foreach (int ch in win.chromosomeArray)
              //  Console.Write(ch);
            //Console.WriteLine();

            //AdjacencyList list = AdjacencyList.GenerateList(win.chromosomeArray, cost.countVillage, cost.countClinic + cost.countMedic);

            //AdjacencyList.PrintGraph(list);
        }

        /// <summary>
        /// Вычесление ранга хромосомы.
        /// </summary>
        /// <param name="chromosome">Хромосома.</param>
        /// <param name="selectionPressure">Селективное давение.</param>
        /// <param name="sizePopulation">Размер популяции.</param>
        /// <param name="indexCh">Индекс хромосомы.</param>
        private double Ranking(Chromosome chromosome, int sizePopulation, int indexCh)
        {
            double a = Math.Round(Utility.Rand.NextDouble() + 1.0, 3);
            double b = Math.Round(2 - a, 3);
            double c = (1.0 / sizePopulation);
            double d = (double)indexCh / (double)(sizePopulation - 1);
            double f = Math.Round(a - (a - b));
            chromosome.rank = c * (f * (d));               
            return chromosome.rank;
        }
             
    }
}
