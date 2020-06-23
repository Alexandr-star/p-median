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

        public AlgorithmInfo algorithmInfo;
        private Stopwatch stopwatch;

        private AdjacencyList adjacencyList;

        private Cost cost;
        private ProblemData problem;

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

        public ClassicGA(int IterationSize, int PopulationSize,
            CrossoverMethod crossoverMethod, double CrossoverProbability, int dotCrossover,
            MutationMethod mutationMethod, double MutationProbability, int dotMutation, SelectionMethod selectionMethod,
            int CountSelected, int CountTour) 
            : base (IterationSize, PopulationSize) 
        {
            this.crossoverMethod = crossoverMethod;
            this.CrossoverProbability = CrossoverProbability;
            this.mutationMethod = mutationMethod;
            this.MutationProbability = MutationProbability;
            this.dotCrossover = dotCrossover;
            this.dotMutation = dotMutation;
            this.selectionMethod = selectionMethod;
            this.CountSelected = CountSelected;
            this.CountTour = CountTour;
        }

        public override int GeneticAlgorithm(MainGraph graph, ProblemData problemData)
        {
            algorithmInfo = new AlgorithmInfo();
            adjacencyList = AdjacencyList.GenerateList(graph);
            cost = Cost.GanerateCostArray(graph, problemData); problem = problemData;

            var crossover = GeneticMethod.ChosenCrossoverMethod(crossoverMethod, CrossoverProbability, dotCrossover);
            var mutation = GeneticMethod.ChosenMutationMethod(mutationMethod, MutationProbability, dotMutation);
            var selection = GeneticMethod.ChosenSelectionMethod(selectionMethod, CountTour, CountSelected);
            ReductionForClassicGA reduction = new ReductionForClassicGA();
                       
            Chromosome bestChromosome = null;
            Population startPopulation = new Population(PopulationSize, cost);
            var population = startPopulation;
            int stepGA = 0;

            double MediumFitness = Solution.MediumFitnessPopulation(population);
            RandomSelection randomSelection = new RandomSelection();
            FitnessCalculation(population.populationList);
            stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stepGA < IterateSize)
            {
                List<Chromosome> midPopulation = selection.Selection(population);
                List<Chromosome> parentPopulation = randomSelection.Selection(midPopulation, selection.indexSelectChrom);
                List<Chromosome> childList = crossover.Crossover(parentPopulation);
                if (childList.Count == 0)
                    continue;
                mutation.Mutation(childList);
                FitnessCalculation(childList);
                    

                reduction.Reduction(childList, randomSelection.indexTwoParant, population.populationList);

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
                            break;
                        }
                    }
                }
                stepGA++;
            }
            stopwatch.Stop();
            if (stepGA == IterateSize)
            {
                bool answer = false;
                bestChromosome = population.BestChromosome();
                while (population.populationList.Count != 0)
                {
                        
                    if (Solution.isAnswerTrue(bestChromosome, cost, problemData))
                    {                          
                        algorithmInfo.Time = stopwatch.Elapsed;
                        algorithmInfo.BestFx = bestChromosome.fitness;
                        algorithmInfo.Steps = stepGA;
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
                    bestChromosome = null;
                }
            }
                                
            return Solution.Answer(cost, bestChromosome, problemData, graph);

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
            }
        }

    }
}
