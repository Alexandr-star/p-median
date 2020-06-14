﻿using Pmedian.CoreData.DataStruct;
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
        public override AdjacencyList GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            this.algorithmInfo = new AlgorithmInfo();
            // Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.CreateCostArray(graph);
            ProblemData problem = problemData;
            
            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, 100, dotCrossover, minHemmingDistance);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability, dotMutation);
            Chromosome bestChromosome = null;
            double MediumFitness = 0;


            stopwatch = new Stopwatch();
            stopwatch.Start();
            Population startPopulation = new Population(PopulationSize, cost);
            Console.WriteLine($"fit {Fitness.FunctionTrue(cost, problemData, startPopulation.populationList[0])}");

            var population = startPopulation;
           

            int stepGA = 0;           

            // вычесление пригодности хромосом.
            //Console.WriteLine("fitness ");
            for (int i = 0; i < PopulationSize; i++)
            {
                population.populationList[i].fitness = Fitness.FunctionTrue(cost, problemData, population.populationList[i]);
                //Console.WriteLine(population.populationList[i].fitness);
            }
            MediumFitness = Solution.MediumFitnessPopulation(population);

            while (stepGA <= IterateSize)
            {
                // выбор двух хромосом для скрещивания
                List<Chromosome> selectedChromosome = RandomSelection.Selection(population.populationList);
                // полученный потомок после скрещивания

                Chromosome child = crossover.Crossover(selectedChromosome[0], selectedChromosome[1]);
               
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
                            algorithmInfo.Time = stopwatch.Elapsed;
                            algorithmInfo.BestFx = bestChromosome.fitness;
                            algorithmInfo.Steps = stepGA;
                            bestChromosome.PrintChromosome();

                            break;
                        }
                    }
                }
                

                stepGA++;
            }
            stopwatch.Stop();

            if (stepGA == IterateSize)
            {
                bestChromosome = population.BestChromosome();
                if (Solution.isAnswer(bestChromosome, cost, problemData))
                {
                    algorithmInfo.Time = stopwatch.Elapsed;
                    algorithmInfo.BestFx = bestChromosome.fitness;
                    algorithmInfo.Steps = stepGA;
                    bestChromosome.PrintChromosome();

                }
            }
            else if (bestChromosome == null)
            {
                while (population.populationList.Count != 0)
                {
                    bestChromosome = population.BestChromosome();
                    if (Solution.isAnswerTrue(bestChromosome, cost, problemData))
                    {
                        stopwatch.Stop();

                        algorithmInfo.Time = stopwatch.Elapsed;
                        algorithmInfo.BestFx = bestChromosome.fitness;
                        algorithmInfo.Steps = stepGA;
                        break;
                    }
                    else
                        population.populationList.Remove(bestChromosome);
                }
            }
            else
            {
                bool answer = false;
                while (population.populationList.Count != 0)
                {
                    if (Solution.isAnswerTrue(bestChromosome, cost, problemData))
                    {
                        algorithmInfo.Time = stopwatch.Elapsed;
                        algorithmInfo.BestFx = bestChromosome.fitness;
                        algorithmInfo.Steps = stepGA;
                        bestChromosome.PrintChromosome();

                        answer = true;
                        break;
                    }
                    else
                    {
                        population.populationList.Remove(bestChromosome);
                        bestChromosome = population.BestChromosome();
                    }
                }
                if (!answer)
                {
                    bestChromosome = null;

                }
            }


            return AdjacencyList.GenerateList(bestChromosome, cost);

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

        /// <summary>
        /// Проверка ранга.
        /// </summary>
        /// <param name="population">Популяция.</param>
        /// <returns></returns>
        private bool checkRank(Population population)
        {
            
            double n = PopulationSize * (PopulationSize + 1) / 2;
            double N = 0;
            foreach (var ch in population.populationList)
                N += ch.rank;
            if (n == N)
                return true;
            else
                return false;      
        }

        public override AlgorithmInfo GetAlgorithmInfo()
        {
            return algorithmInfo;
        }
    }
}
