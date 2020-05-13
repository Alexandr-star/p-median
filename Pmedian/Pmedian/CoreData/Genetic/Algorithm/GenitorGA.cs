using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Selection;
using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


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
            startPopulation.PrintPopulation();

            foreach(var chromosome in startPopulation.populationList)
            {
                chromosome.fitness = Fitness.Function(cost, problemData, chromosome);
                Console.WriteLine(chromosome.fitness);
            }
            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, CrossoverProbability);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability);

            var population = startPopulation;

            int stepGA = 1;
            
            while(stepGA < IterateSize)
            {

                // вычесление пригодности хромосом.
                for(int i = 0; i < PopulationSize; i++)
                {
                    population.populationList[i].fitness = Fitness.Function(cost, problemData, population.populationList[i]);
                }
                
                // выбор двух хромосом для скрещивания
                List<Chromosome> selectedChromosome = RandomSelection.Selection(population.populationList);
                // полученный потомок после скрещивания
                var child = crossover.Crossover(selectedChromosome[0], selectedChromosome[1]);
                // пригодность потомка
                child.fitness = Fitness.Function(cost, problemData, child);
                // поиск самой худщей хромосомы
                var badChrom = population.WorstChromosome();
                // замена худшей хромосомы на потомка
                if (child.fitness < badChrom.fitness)
                {
                    int index = population.populationList.IndexOf(badChrom);
                    population.populationList.RemoveAt(index);
                    population.populationList.Insert(index, child);
                }
                stepGA++;

            }
            Console.WriteLine("answer");
            Chromosome winCh = population.BestChromosome();
            Console.WriteLine(winCh.fitness);
            int indexWin = population.populationList.IndexOf(winCh);


            Chromosome win = population.populationList.ElementAt(indexWin);
            foreach(int ch in win.chromosomeArray) 
                Console.Write(ch);
            Console.WriteLine();
            
            AdjacencyList list = AdjacencyList.GenerateList(win.chromosomeArray, cost.countVillage, cost.countClinic + cost.countMedic);
            
            AdjacencyList.PrintGraph(list);
        }
    }
}
