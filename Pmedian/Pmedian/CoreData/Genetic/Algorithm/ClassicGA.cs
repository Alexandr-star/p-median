using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Function;
using Pmedian.CoreData.Genetic.Mutation;
using Pmedian.CoreData.Genetic.Reduction;
using Pmedian.CoreData.Genetic.Selection;
using Pmedian.CoreData.Genetic.Сrossover;
using Pmedian.Model;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class ClassicGA : AbstractGeneticAlgorithm
    {
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
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.CreateCostArray(graph);
            problem = problemData;
            Console.WriteLine("Problem");
            Console.WriteLine(problem.P);
            Console.WriteLine(problem.RoadCost);
            Console.WriteLine(problem.TimeAmbulance);
            Console.WriteLine(problem.TimeMedic);
            Population startPopulation = new Population(PopulationSize, cost);
            OneDotCrossover crossover = new OneDotCrossover(CrossoverProbability);
            ReplaceMutaion mutaion = new ReplaceMutaion(MutationProbability, 1);
            TournamentSelection selection = new TournamentSelection(CountSelected, CountTour);
            ReductionForClassicGA reduction = new ReductionForClassicGA();
            var population = startPopulation;
            int stepGA = 0;
            Chromosome bestChromosome = null;
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
                            Console.WriteLine(" ANSVER");
                            break;
                        }
                    }
                }

                stepGA++;
            }

            Console.WriteLine($"answer, step {stepGA}");
            //population.PrintPopulation();

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
            foreach (var chromosom in chromosomes)
            {
                 chromosom.fitness = Fitness.Function(cost, problem, chromosom);
                //Console.WriteLine(population.populationList[i].fitness);
            }
        }

    }
}
