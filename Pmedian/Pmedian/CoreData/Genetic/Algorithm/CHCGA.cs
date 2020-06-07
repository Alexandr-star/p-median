using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Mutation;
using Pmedian.CoreData.Genetic.Reduction;
using Pmedian.CoreData.Genetic.Selection;
using Pmedian.CoreData.Genetic.Сrossover;
using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        private int STAGNATION = 5;

        /// <summary>
        /// Таблица расходов.
        /// </summary>
        private Cost cost;

        /// <summary>
        /// Параметры задачи.
        /// </summary>
        private ProblemData problem;

        /// <summary>
        /// Вероятность кроссовера.
        /// </summary>
        private double CrossoverProbability;

        /// <summary>
        /// Метод мутации для катаклизма.
        /// </summary>
        private MutationMethod mutationMethod;

        /// <summary>
        /// Точек в мутации.
        /// </summary>
        private int dotMutation;

        /// <summary>
        /// Минимальное Хемминговое растояние между родителями
        /// </summary>
        private int minHemmingDistance;
       
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="IterationSize">Количество итераций ГА.</param>
        /// <param name="PopulationSize">Размер популяции.</param>
        /// <param name="crossoverMethod">Оператор кроссовера.</param>
        /// <param name="CrossoverProbability">Вероятность кроссовера.</param>
        public CHCGA(int IterationSize, int PopulationSize,
            double CrossoverProbability,
            MutationMethod mutationMethod, int dotMutation,
            int minHemmingDistance)
            : base(IterationSize, PopulationSize)
        {
            this.CrossoverProbability = CrossoverProbability;
            this.mutationMethod = mutationMethod;
            this.minHemmingDistance = minHemmingDistance;
            this.dotMutation = dotMutation;
        }

        /// <summary>
        /// Реализация генетического алгоритма "CHC".
        /// </summary>
        /// <param name="graph">Граф.</param>
        /// <param name="problemData">Параметры задачи.</param>
        public override AdjacencyList GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            Console.WriteLine("CHC");
            // Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.CreateCostArray(graph);
            problem = problemData;
            Console.WriteLine("Problem");
            Console.WriteLine(problem.P);
            Console.WriteLine(problem.RoadCost);
            Console.WriteLine(problem.TimeAmbulance);
            Console.WriteLine(problem.TimeMedic);
            Population startPopulation = new Population(PopulationSize, cost);

            bool d = DoCtaclism(startPopulation);

            var population = startPopulation;
            Chromosome bestChromosome = null;
            HUXCrossover crossover = new HUXCrossover(minHemmingDistance, CrossoverProbability);
            int stepGA = 0;
            FitnessCalculation(population.populationList);
            double MediumFitness = Solution.MediumFitnessPopulation(population);
            int staticPop = 0;
            population.PrintPopulation();
            while (stepGA < IterateSize)
            {
                List<Chromosome> childList = crossover.Crossover(population.populationList);
                if (childList.Count == 0)
                    crossover.minHemmingDistanse = --minHemmingDistance;
                FitnessCalculation(childList);
                if (childList.Count < population.SizePopulation)
                    Console.WriteLine($"childList {childList.Count}");
                
                population.populationList = EliteReductionForCHC.Reduction(population.populationList, childList, PopulationSize);

                double tempMediumFitness = Solution.MediumFitnessPopulation(population);
                double absFitness = tempMediumFitness - MediumFitness;
                MediumFitness = tempMediumFitness;
                if (absFitness == 0.0)
                    staticPop++;
                else
                    staticPop = 0;
                
                if (staticPop == STAGNATION)
                {
                    if (DoCtaclism(population))
                    {
                        Console.WriteLine($"cut");

                        CatacliysmicMutation(population);
                    }
                }
                
              

                stepGA++;
            }
            Console.WriteLine($"answer, step {stepGA}");
            population.PrintPopulation();
            if (bestChromosome == null)
            {
                int index = 0;
                Console.WriteLine("Null best");
                while (population.populationList.Count != 0)
                {
                    bestChromosome = population.BestChromosome();
                    if (Solution.isAnswer(bestChromosome, cost, problemData, index))
                    {
                        Console.WriteLine(" ANSVER");
                        break;
                    }
                    else
                        population.populationList.Remove(bestChromosome);
                    index++;
                }                
            }
            if (Solution.isAnswer(bestChromosome, cost, problemData))
            {
                Console.WriteLine(bestChromosome.fitness);
                bestChromosome.PrintChromosome();
                Console.WriteLine(" ANSVER");
            }
            else
            {
                bestChromosome = null;
                Console.WriteLine(" NO ANSVER");

            }
            
            return AdjacencyList.GenerateList(bestChromosome, cost);
        }

        /// <summary>
        /// Вычесление пригодности хромосом.
        /// </summary>
        /// <param name="chromosomes">Список хромосом</param>
        private void FitnessCalculation(List<Chromosome> chromosomes)
        {
            // Вычесление пригодности хромосом.
            foreach (var chromosom  in chromosomes)
            {
                chromosom.fitness = Fitness.Function(cost, problem, chromosom);
                //Console.WriteLine(population.populationList[i].fitness);
            }
        }

        private bool DoCtaclism(Population population)
        {
                      
            var list = population.populationList.GroupBy(ch => string.Join(string.Empty, ch.chromosomeArray)).Select(ch => ch.First()).ToList();
            if (population.populationList.Count > list.Count)
                return true;
            
            return false;
        }
        

        /// <summary>
        /// Мутация при которой все особоби, кроме самой лучшей мутируют 
        /// (мутируют около 1/3 битов).
        /// </summary>
        /// <param name="population">Популяция.</param>
        private void CatacliysmicMutation(Population population)
        {
            Chromosome bestChromosome = population.BestChromosome();
            int bitsMutation = (int)(population.SizeChromosome * 0.35);
            AbstractMutation mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, 100, bitsMutation);
            
            foreach (var chromosome in population.populationList)
            {
                if (!chromosome.Equals(bestChromosome))
                {
                    mutation.Mutation(chromosome);
                }
            }            
        }
    }
}
