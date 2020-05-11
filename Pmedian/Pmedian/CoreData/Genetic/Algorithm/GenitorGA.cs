using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Mutation;
using Pmedian.CoreData.Genetic.Selection;
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
    class GenitorGA : AbstractGeneticAlgorithm
    {
        /// <summary>
        /// Граф, задаваемый списком смежности вершин.
        /// </summary>
        private AdjacencyList adjacencyList;

        private Cost costArray;


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
            costArray = new Cost(10);
            Population startPopulation = new Population(PopulationSize, costArray);

            startPopulation.PrintPopulation();
            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, CrossoverProbability);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability);
            int[] fitnessChromosomes = new int[PopulationSize];

            List<int[]> population = startPopulation.populationList;

            int stepGA = 1;
            while(true)
            {

                // вычесление пригодности хромосом.
                for(int i = 0; i < PopulationSize; i++)
                {
                    fitnessChromosomes[i] = Fitness.Function(costArray.TESTcostEdgeArray, population[i]);
                }
                if (stepGA == IterateSize) return;
                
                

                // выбор двух хромосом для скрещивания
                List<int[]> selectedChromosome = RandomSelection.Selection(population);
                // полученный потомок после скрещивания
                int[] child = crossover.Crossover(selectedChromosome[0], selectedChromosome[1]);
                // пригодность потомка
                int fitChild = Fitness.Function(costArray.TESTcostEdgeArray, child);
                // поиск самой худщей хромосомы
                int badChrom = fitnessChromosomes.Min();
                // замена худшей хромосомы на потомка
                if (fitChild < badChrom)
                {
                    int index = Array.IndexOf(fitnessChromosomes, badChrom);
                    population.RemoveAt(index);
                    population.Insert(index, child);
                }

            }          
        }

        
    }
}
