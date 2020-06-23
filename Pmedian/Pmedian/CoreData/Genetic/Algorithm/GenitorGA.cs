using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Selection;
using Pmedian.Model;
using Pmedian.Model.Enums;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Navigation;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class GenitorGA : AbstractGeneticAlgorithm
    {
        private int TESTITER = 100;
        public AlgorithmInfo algorithmInfo;

        private Stopwatch stopwatch;
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

        private SelectionMethod selectionMethod;
        private int CountSelected;

        private int CountTour;

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
            SelectionMethod selectionMethod, int CountSelected, int CountTour)
            : base(IterationSize, PopulationSize)
        {
            this.crossoverMethod = crossoverMethod;
            this.CrossoverProbability = CrossoverProbability;
            this.mutationMethod = mutationMethod;
            this.MutationProbability = MutationProbability;
            this.dotCrossover = dotCrossover;
            this.dotMutation = dotMutation;
            this.selectionMethod = SelectionMethod.Proportion;
            this.CountSelected = CountSelected;
            this.CountTour = CountTour;
        }

        /// <summary>
        /// Реализация генетического алгоритма "Genitor".
        /// </summary>
        /// <param name="graph">Граф.</param>
        /// <param name="problemData">Параметры задачи</param>
        public override int GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            this.algorithmInfo = new AlgorithmInfo();
            // Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.GanerateCostArray(graph, problemData); ProblemData problem = problemData;

            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, 100, dotCrossover);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability, dotMutation);
            var selection = GeneticMethod.ChosenSelectionMethod(selectionMethod, CountTour, CountSelected);
            Chromosome bestChromosome = null;
            double MediumFitness = 0;
            double midTime = .0;
            double midBestFit = .0;
            int midIter = 0;
            int iter = 0;
            int countAnswer = 0;

            while (iter < TESTITER) {
                Population startPopulation = new Population(PopulationSize, cost);

                var population = startPopulation;


                int stepGA = 0;

                // вычесление пригодности хромосом.
                //Console.WriteLine("fitness ");
                for (int i = 0; i < PopulationSize; i++)
                {
                    population.populationList[i].fitness = Fitness.FunctionTrue(cost, problemData, population.populationList[i]);
                    //Console.WriteLine(population.populationList[i].fitness);
                }
                RandomSelection randomSelection = new RandomSelection();
                MediumFitness = Solution.MediumFitnessPopulation(population);
                stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stepGA < IterateSize)
                {
                    List<Chromosome> midPopulation = selection.Selection(population);
                    List<Chromosome> parentPopulation = randomSelection.Selection(midPopulation);                   
                    Chromosome child = crossover.Crossover(parentPopulation[0], parentPopulation[1]);
                    
                    if (mutation != null)
                    {
                        mutation.Mutation(child);
                    }
                    // вычесление ранга хромосомы.

                    population.Sort();
                    Ranking(population);
                    // пригодность потомка
                    child.fitness = Fitness.FunctionTrue(cost, problemData, child);
                    // поиск самой худщей хромосомы
                    Chromosome bedChrom = null;
                    if (stepGA == 0)
                        bedChrom = population.ChromosomeWithMinRank();
                    else
                        bedChrom = population.OneOfChromosomesWithMinRank();
                    
                    // замена худшей хромосомы на потомка
                    int index = population.populationList.IndexOf(bedChrom);
                    population.populationList.Insert(index, child);
                    population.populationList.RemoveAt(index + 1);

                    double tempMediumFitness = Solution.MediumFitnessPopulation(population);
                    double absFitness = Math.Abs(tempMediumFitness - MediumFitness);
                    MediumFitness = tempMediumFitness;
                    if (absFitness >= 0 && absFitness <= 1)
                    {
                        bestChromosome = population.BestChromosome();
                        var worstChromosome = population.WorstChromosome();
                        if (bestChromosome.fitness - worstChromosome.fitness <= 1 && bestChromosome.fitness - worstChromosome.fitness >= 0)
                        {
                            if (Solution.isAnswerTrue(bestChromosome, cost, problemData))
                            {
                                stopwatch.Stop();
                                double sol = Solution.isAnswer(bestChromosome, cost, problemData);

                                midTime += stopwatch.Elapsed.TotalSeconds;
                                midBestFit += sol;
                                midIter += stepGA;
                                countAnswer++;

                                algorithmInfo.Time = stopwatch.Elapsed;
                                algorithmInfo.BestFx = bestChromosome.fitness;
                                algorithmInfo.Steps = stepGA;
                                Console.WriteLine(" ANSVER");
                                bestChromosome.PrintChromosome();
                                Console.WriteLine($"F = {sol}");
                                Console.WriteLine($"best {bestChromosome.fitness}");

                                break;
                            }
                        }
                    }


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
                            double sol = Solution.isAnswer(bestChromosome, cost, problemData);
                            midTime += stopwatch.Elapsed.TotalSeconds;
                            midBestFit += sol;
                            midIter += stepGA;
                            countAnswer++;

                            algorithmInfo.Time = stopwatch.Elapsed;
                            algorithmInfo.BestFx = bestChromosome.fitness;
                            algorithmInfo.Steps = stepGA;
                            Console.WriteLine(" ANSVER");
                            bestChromosome.PrintChromosome();

                            Console.WriteLine($"F = {sol}");
                            Console.WriteLine($"best {bestChromosome.fitness}");

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
                
                Console.WriteLine($"iter {iter}");

                iter++;
            }
        
            Console.WriteLine($"mid time: {midTime / TESTITER}");
            Console.WriteLine($"mid fit: b/iter {midBestFit / TESTITER}  b/answ {midBestFit / countAnswer}");
            Console.WriteLine($"mid iter: {midIter / TESTITER}");
            Console.WriteLine($"count answer {countAnswer}/{TESTITER}");

            return Solution.Answer(cost, null, problemData, graph);

        }

        /// <summary>
        /// Вычесление ранга хромосом.
        /// </summary>
        /// <param name="population">Популяция.</param>
        private void Ranking(Population population)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                population.populationList[i].rank = i + 1;
            }

            int count = 0;
            double sumRank = 0;
            bool isDuplo = false;
            for (int i = 0; i < PopulationSize; i++)
            {
                if (i == PopulationSize - 1)
                {
                    if (isDuplo)
                    {
                        count++;
                        sumRank += population.populationList[i].rank;
                        double rank = sumRank / count;
                        for (int j = i - count + 1; j < i + 1; j++)
                        {
                            population.populationList[j].rank = rank;
                        }
                    }
                }
                else if (population.populationList[i].fitness == population.populationList[i + 1].fitness)
                {
                    sumRank += population.populationList[i].rank;
                    count++;
                    isDuplo = true;
                }
                else if (isDuplo)
                {
                    count++;
                    sumRank += population.populationList[i].rank;
                    double rank = sumRank / count;
                    for (int j = i - count + 1; j < i + 1; j++)
                    {
                        population.populationList[j].rank = rank;
                    }
                    sumRank = 0;
                    count = 0;
                    isDuplo = false;
                }
            }
        }

        public override AlgorithmInfo GetAlgorithmInfo()
        {
            return algorithmInfo;
        }
    } 
      
}

