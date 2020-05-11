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
        public override void GeneticAlgorithm(MainGraph graph)
        {
            //Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.CreateCostArray(graph);
            Population startPopulation = new Population(PopulationSize, cost);
            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, CrossoverProbability);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability);
            int[] fitnessChromosomes = new int[PopulationSize];

            List<int[]> population = startPopulation.populationList;

            int stepGA = 1;
            
            while(stepGA < IterateSize)
            {

                // вычесление пригодности хромосом.
                for(int i = 0; i < PopulationSize; i++)
                {
                    fitnessChromosomes[i] = Fitness.Function(cost.TESTcostEdgeArray, population[i]);
                }
                
                // выбор двух хромосом для скрещивания
                List<int[]> selectedChromosome = RandomSelection.Selection(population);
                // полученный потомок после скрещивания
                int[] child = crossover.Crossover(selectedChromosome[0], selectedChromosome[1]);
                // пригодность потомка
                int fitChild = Fitness.Function(cost.TESTcostEdgeArray, child);
                // поиск самой худщей хромосомы
                int badChrom = fitnessChromosomes.Max();
                // замена худшей хромосомы на потомка
                if (fitChild < badChrom)
                {
                    int index = Array.IndexOf(fitnessChromosomes, badChrom);
                    population.RemoveAt(index);
                    population.Insert(index, child);
                }
                stepGA++;
                Console.WriteLine($"step {stepGA}");

            }
            Console.WriteLine("answer");
            int winCh = fitnessChromosomes.Min();
            Console.WriteLine(winCh);
            int indexWin = Array.IndexOf(fitnessChromosomes, winCh);

            
            int[] win = population.ElementAt(indexWin);
            foreach(int ch in win) 
                Console.Write(ch);
            Console.WriteLine();
            AdjacencyList list = AdjacencyList.GenerateList(win, cost.countVillage, cost.countOtherPoint);
            
            AdjacencyList.PrintGraph(list);
        }
    }
}
