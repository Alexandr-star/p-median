using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Mutation;
using Pmedian.CoreData.Genetic.Reduction;
using Pmedian.CoreData.Genetic.Selection;
using Pmedian.CoreData.Genetic.Сrossover;
using Pmedian.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class ClassicGA : AbstractGeneticAlgorithm
    {
        public AlgorithmInfo algorithmInfo;
        private Stopwatch stopwatch;

        private AdjacencyList adjacencyList;

        private Cost cost;
        private ProblemData problem;

        private double CrossoverProbability;

        private double MutationProbability;

        private int CountSelected;

        private int CountTour;

        public ClassicGA(int IterationSize, int PopulationSize,
            double CrossoverProbability,
            double MutationProbability,
            int CountSelected, int CountTour) 
            : base (IterationSize, PopulationSize) 
        {
            this.CrossoverProbability = CrossoverProbability;
            this.MutationProbability = MutationProbability;
            this.CountSelected = CountSelected;
            this.CountTour = CountTour;
        }

        public override AdjacencyList GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            algorithmInfo = new AlgorithmInfo();
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.CreateCostArray(graph);
            problem = problemData;
            Console.WriteLine("Problem");
            Console.WriteLine(problem.P);
            Console.WriteLine(problem.RoadCost);
            Console.WriteLine(problem.TimeAmbulance);
            Console.WriteLine(problem.TimeMedic);
            OneDotCrossover crossover = new OneDotCrossover(CrossoverProbability);
            ReplaceMutaion mutaion = new ReplaceMutaion(MutationProbability, 1);
            TournamentSelection selection = new TournamentSelection(CountSelected, CountTour);
            ReductionForClassicGA reduction = new ReductionForClassicGA();
            Chromosome bestChromosome = null;
            

            stopwatch = new Stopwatch();
            stopwatch.Start();
            Population startPopulation = new Population(PopulationSize, cost);
            var population = startPopulation;
            int stepGA = 0;
           
            double MediumFitness = Solution.MediumFitnessPopulation(population);

            FitnessCalculation(population.populationList);
            population.PrintPopulation();
            while (stepGA < IterateSize)
            {
                List<Chromosome> parentPopulation = selection.Selection(population.populationList);
                List<Chromosome> childList = crossover.Crossover(parentPopulation);
                mutaion.Mutation(childList);                
                FitnessCalculation(childList);
                reduction.Reduction(childList, parentPopulation, population.populationList, crossover.nonp);
                double tempMediumFitness = Solution.MediumFitnessPopulation(population);
                double absFitness = Math.Abs(tempMediumFitness - MediumFitness);
                MediumFitness = tempMediumFitness;
                if (absFitness >= 0 && absFitness <= 1)
                {
                    bestChromosome = population.BestChromosome();
                    var worstChromosome = population.WorstChromosome();
                    if (bestChromosome.fitness - worstChromosome.fitness <= 1 && bestChromosome.fitness - worstChromosome.fitness >= 0)
                    {
                        Console.WriteLine("сошелся");
                        if (Solution.isAnswer(bestChromosome, cost, problemData))
                        {
                            stopwatch.Stop();
                            algorithmInfo.Time = stopwatch.Elapsed;
                            algorithmInfo.BestFx = bestChromosome.fitness;
                            algorithmInfo.Steps = stepGA;
                            Console.WriteLine(" ANSVER");
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
                bestChromosome = population.BestChromosome();
                if (Solution.isAnswer(bestChromosome, cost, problemData))
                {
                    Console.WriteLine(bestChromosome.fitness);
                    Console.WriteLine($"time {stopwatch.Elapsed}  {stopwatch.Elapsed.TotalSeconds}") ;
                    algorithmInfo.Time = stopwatch.Elapsed;
                    algorithmInfo.BestFx = bestChromosome.fitness;
                    algorithmInfo.Steps = stepGA;
                    Console.WriteLine(" ANSVER");
                }
            }
            else if (bestChromosome == null)
            {
                Console.WriteLine("Null best");
                while (population.populationList.Count != 0)
                {
                    bestChromosome = population.BestChromosome();
                    algorithmInfo.Time = stopwatch.Elapsed;
                    algorithmInfo.BestFx = bestChromosome.fitness;
                    algorithmInfo.Steps = stepGA;
                    if (Solution.isAnswer(bestChromosome, cost, problemData))
                    {
                        algorithmInfo.Time = stopwatch.Elapsed;
                        algorithmInfo.BestFx = bestChromosome.fitness;
                        algorithmInfo.Steps = stepGA;
                        Console.WriteLine(" ANSVER");
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
                    if (Solution.isAnswer(bestChromosome, cost, problemData))
                    {
                        algorithmInfo.Time = stopwatch.Elapsed;
                        algorithmInfo.BestFx = bestChromosome.fitness;
                        algorithmInfo.Steps = stepGA;
                        Console.WriteLine(" ANSVER");
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
                    Console.WriteLine("NOT ANSVER");

                }
            }
            

            return AdjacencyList.GenerateList(bestChromosome, cost);
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
                 chromosom.fitness = Fitness.Function(cost, problem, chromosom);
                //Console.WriteLine(population.populationList[i].fitness);
            }
        }

    }
}
