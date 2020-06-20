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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class CHCGA : AbstractGeneticAlgorithm
    {
        private int TESTITER = 500;
        public AlgorithmInfo algorithmInfo;
        private Stopwatch stopwatch;

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
        public override int GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            algorithmInfo = new AlgorithmInfo();
            // Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.GanerateCostArray(graph, problemData);
            problem = problemData;
            Chromosome bestChromosome = null;
            Population startPopulation = new Population(PopulationSize, cost);
            var population = startPopulation;
            HUXCrossover crossover = new HUXCrossover(population.SizeChromosome / 4, CrossoverProbability);
            //double MediumFitness = Solution.MediumFitnessPopulation(population);
            int stepGA = 0;

            double midTime = .0;
            double midBestFit = .0;
            int midIter = 0;
            int countAnswer = 0;

            int iter = 0;
            while (iter < TESTITER){

                stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stepGA < IterateSize)
                {
                    FitnessCalculation(population.populationList);
                    List<Chromosome> childList = crossover.Crossover(population.populationList);


                    if (crossover.Distanse == 0)
                    {
                        CatacliysmicMutation(population);
                        crossover.Distanse = population.SizeChromosome / 4;
                        stepGA++;

                        continue;
                    }
                    if (childList.Count == 0)
                    {
                        crossover.Distanse--;
                        stepGA++;

                        continue;
                    }
                    FitnessCalculation(childList);
                    //TODO изменить элитарный отбор.
                    List<Chromosome> newPopulation = EliteReductionForCHC.Reduction(population.populationList, childList, PopulationSize);

                    population.populationList = newPopulation;

                    stepGA++;
                }
                stopwatch.Stop();
                Console.WriteLine($"answer, step {stepGA}");
                if (stepGA == IterateSize)
                {
                    bool answer = false;
                    bestChromosome = population.BestChromosome();
                    while (population.populationList.Count != 0)
                    {

                        if (Solution.isAnswerTrue(bestChromosome, cost, problemData))
                        {
                            midTime += stopwatch.Elapsed.TotalSeconds;
                            midBestFit += bestChromosome.fitness;
                            midIter += stepGA;
                            countAnswer++;

                            algorithmInfo.Time = stopwatch.Elapsed;
                            algorithmInfo.BestFx = bestChromosome.fitness;
                            algorithmInfo.Steps = stepGA;
                            Console.WriteLine(" ANSVER");
                            bestChromosome.PrintChromosome();

                            answer = true;
                            break;
                        }
                        else
                        {
                            population.populationList.Remove(bestChromosome);
                            if (population.populationList.Count == 0)
                                break;
                            bestChromosome = population.BestChromosome();

                        }
                    }
                    if (!answer)
                    {
                        midTime += stopwatch.Elapsed.TotalSeconds;
                        midBestFit += 0;
                        midIter += stepGA;
                        bestChromosome = null;
                        Console.WriteLine("NOT ANSVER");

                    }
                }

                iter++;
            }
            Console.WriteLine($"mid time: {midTime / TESTITER}");
            Console.WriteLine($"mid fit: b/iter {midBestFit / TESTITER}  b/answ {midBestFit / countAnswer}");
            Console.WriteLine($"mid iter: {midIter / TESTITER}");
            Console.WriteLine($"count answer {2 * countAnswer}/{2 * TESTITER}");
            return Solution.Answer(cost, null, problemData, graph);
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
                chromosom.fitness = Fitness.FunctionTrue(cost, problem, chromosom);
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
            int bitsMutation = (int)(population.SizeChromosome / 2);
            AbstractMutation mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, 100, bitsMutation);
            
            foreach (var chromosome in population.populationList)
            {
                if (!chromosome.Equals(bestChromosome))
                {
                    mutation.Mutation(chromosome);
                }
            }            
        }

        public override AlgorithmInfo GetAlgorithmInfo()
        {
            return algorithmInfo;
        }
    }
}
