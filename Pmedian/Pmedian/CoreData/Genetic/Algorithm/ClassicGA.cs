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
using System.IO;
using System.Windows.Documents;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class ClassicGA : AbstractGeneticAlgorithm
    {
        private int TESTITER = 100;

        public AlgorithmInfo algorithmInfo;
        private Stopwatch stopwatch;

        private AdjacencyList adjacencyList;

        private Cost cost;
        private ProblemData problem;

        private double CrossoverProbability;

        private double MutationProbability;

        private SelectionMethod selectionMethod;

        private int CountSelected;

        private int CountTour;

        public ClassicGA(int IterationSize, int PopulationSize,
            double CrossoverProbability,
            double MutationProbability,
            SelectionMethod selectionMethod,
            int CountSelected, int CountTour) 
            : base (IterationSize, PopulationSize) 
        {
            this.CrossoverProbability = CrossoverProbability;
            this.MutationProbability = MutationProbability;
            this.selectionMethod = selectionMethod;
            this.CountSelected = CountSelected;
            this.CountTour = CountTour;
        }

        public override int GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            algorithmInfo = new AlgorithmInfo();
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.GanerateCostArray(graph, problemData); problem = problemData;
            Console.WriteLine("Problem");
            Console.WriteLine(problem.P);
            Console.WriteLine(problem.RoadCost);

            OneDotCrossover crossover = new OneDotCrossover(CrossoverProbability);
            ReplaceMutaion mutaion = new ReplaceMutaion(MutationProbability, 1);
            var selection = GeneticMethod.ChosenSelectionMethod(selectionMethod, CountTour, CountSelected);
            ReductionForClassicGA reduction = new ReductionForClassicGA();
            
            double midTime = .0;
            double midBestFit = .0;
            int midIter = 0;
            int countAnswer = 0;

            int iter = 0;
            while (iter < TESTITER)
            {
                Chromosome bestChromosome = null;


                Population startPopulation = new Population(PopulationSize, cost);

                var population = startPopulation;
                int stepGA = 0;

                double MediumFitness = Solution.MediumFitnessPopulation(population);

                FitnessCalculation(population.populationList);
                stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stepGA < IterateSize)
                {
                    List<Chromosome> midPopulation = selection.Selection(population);
                    List<Chromosome> parentPopulation = RandomSelection.Selection(midPopulation);
                    List<Chromosome> childList = crossover.Crossover(parentPopulation);
                    if (childList.Count == 0)
                        continue;
                    mutaion.Mutation(childList);
                    FitnessCalculation(childList);
                    reduction.Reduction(childList, parentPopulation, population.populationList);
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

                                midTime += stopwatch.Elapsed.TotalSeconds;
                                midBestFit += bestChromosome.fitness;
                                midIter += stepGA;
                                countAnswer++;

                                algorithmInfo.Time = stopwatch.Elapsed;
                                algorithmInfo.BestFx = bestChromosome.fitness;
                                algorithmInfo.Steps = stepGA;
                                Console.WriteLine(" ANSVER");
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
                            midTime += stopwatch.Elapsed.TotalSeconds;
                            midBestFit += bestChromosome.fitness;
                            midIter += stepGA;
                            countAnswer++;

                            algorithmInfo.Time = stopwatch.Elapsed;
                            algorithmInfo.BestFx = bestChromosome.fitness;
                            algorithmInfo.Steps = stepGA;
                            Console.WriteLine(" ANSVER");
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
            Console.WriteLine($"count answer {10*countAnswer}/{10*TESTITER}");
            return Solution.Answer(cost, null, problemData, graph);

        }

        
        public override AlgorithmInfo GetAlgorithmInfo()
        {
            return algorithmInfo;
        }

        /// <summary>
        /// Вычесление пригодности хромосом.
        /// </summary>
        /// <param name="chromosomes">Список хромосом</param>
        private void FitnessCalculation(List<Chromosome> chromosomes)
        {
            // Вычесление пригодности хромосом.
            foreach (var chromosom in chromosomes)
            {
                 chromosom.fitness = Fitness.FunctionTrue(cost, problem, chromosom);
                //Console.WriteLine(population.populationList[i].fitness);
            }
        }

    }
}
