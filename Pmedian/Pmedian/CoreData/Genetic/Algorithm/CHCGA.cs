using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Mutation;
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
        private int countStagnation = 0;
        private int count = 0;

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
        public override void GeneticAlgorithm(MainGraph graph, ProblemData problemData)
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

            HUXCrossover crossover = new HUXCrossover(minHemmingDistance, CrossoverProbability);

            var population = startPopulation;
            Chromosome bestChromosome = null;
            double MediumFitness = 0;

            int stepGA = 0;
            
            while (stepGA <= IterateSize)
            {
                FitnessCalculation(population.populationList);
                MediumFitness = Solution.MediumFitnessPopulation(population);
                //population.PrintPopulation();
                
                List<Chromosome> childList = crossover.Crossover(population.populationList);
                FitnessCalculation(childList);
                population.populationList = ReductionIntermediatePopulation(population.populationList, childList);
                double tempMediumFitness = Solution.MediumFitnessPopulation(population);
                //population.PrintPopulation();

                double absFitness = Math.Abs(tempMediumFitness - MediumFitness);
                if (absFitness >= 0 && absFitness <= 1)
                {
                    countStagnation++;
                    Console.WriteLine(countStagnation);
                    
                }
                else
                    countStagnation = 0;

                if (countStagnation >= STAGNATION)
                {
                    Console.WriteLine("YES");
                    bestChromosome = population.BestChromosome();
                    if (Solution.isAnswer(bestChromosome, cost, problemData))
                        break;
                    else
                    {
                        Console.WriteLine("cataclizm");
                        CatacliysmicMutation(population);
                        count = 0;
                    }
                }
                   
                if (count >= PopulationSize / 4)
                {
                    Console.WriteLine("cataclizm");
                    CatacliysmicMutation(population);
                    count = 0;
                }
                stepGA++;
            }
            Console.WriteLine($"answer, step {stepGA}");
            population.PrintPopulation();
            if (bestChromosome == null)
            {
                Console.WriteLine("Null best");
                while (population.populationList.Count != 0)
                {
                    bestChromosome = population.BestChromosome();
                    if (Solution.isAnswer(bestChromosome, cost, problemData))
                    {
                        Console.WriteLine(" ANSVER");
                        break;
                    }
                    else
                        population.populationList.Remove(bestChromosome);
                }                
            }
            Console.WriteLine(bestChromosome.fitness);
            foreach (int ch in bestChromosome.chromosomeArray)
                Console.Write(ch);
            Console.WriteLine();
            AdjacencyList.GenerateList(bestChromosome, cost);
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


        private List<Chromosome> ReductionIntermediatePopulation(List<Chromosome> populationList, List<Chromosome> childList)
        {
            List<Chromosome> list = new List<Chromosome>();

            list.AddRange(populationList);
            list.AddRange(childList);
            for (int i = 0; i < list.Count - 1; i++)
            {
                double min = list[i].fitness;
                int minId = i;
                for (int j = i + 1; j < list.Count; j++)
                {
                    double temp = list[j].fitness;
                    if (temp < min)
                    {
                        min = temp;
                        minId = j;
                    }
                }

                Chromosome tempChromosome = list[i];
                list.Insert(i, list[minId]);
                list.RemoveAt(i + 1);
                list.Insert(minId, tempChromosome);
                list.RemoveAt(minId + 1);

            }

            List<Chromosome> distList = list.GroupBy(ch => ch.chromosomeArray).Select(ch => ch.First()).ToList();            
            if (distList.Count < PopulationSize)
            {
                count++;
                int diff = PopulationSize - distList.Count;
                for (int i = 0; i < diff; i++)
                    distList.Add(list[i]);
            } else
                distList.RemoveRange(PopulationSize, distList.Count - PopulationSize);
            return distList;
        }

        /// <summary>
        /// Мутация при которой все особоби, кроме самой лучшей мутируют 
        /// (мутируют около 1/3 битов).
        /// </summary>
        /// <param name="population">Популяция.</param>
        private void CatacliysmicMutation(Population population)
        {
            Chromosome bestChromosome = population.BestChromosome();
            int bitsMutation = population.SizeChromosome / 3;
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
