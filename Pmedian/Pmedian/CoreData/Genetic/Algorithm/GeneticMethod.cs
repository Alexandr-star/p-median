using Pmedian.CoreData.Genetic.Mutation;
using Pmedian.CoreData.Genetic.Сrossover;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    public static class GeneticMethod
    {
        public static AbstractCrossover ChosenCrossoverMethod(CrossoverMethod crossoverMethod, double CrossoverProbability)
        {
            AbstractCrossover crossover = null;
            switch (crossoverMethod)
            {
                case CrossoverMethod.OneDot:
                    crossover = new OneDotCrossover(CrossoverProbability);
                    break;
                case CrossoverMethod.HUX:
                    crossover = new HUXCrossover(CrossoverProbability);
                    break;
                case CrossoverMethod.NDot:
                    crossover = new NDotCrossover(1, CrossoverProbability);
                    break;
            }

            return crossover;
        }

        public static AbstractMutation ChosenMutationMethod(MutationMethod mutationMethod, double MutationProbability)
        {
            AbstractMutation mutation = null;
            switch (mutationMethod)
            {
                case MutationMethod.InversionMutation:
                    mutation = new InversionMutation(MutationProbability, 3);
                    break;
                case MutationMethod.ReplaceMutation:
                    mutation = new ReplaceMutaion(MutationProbability, 1);
                    break;
                case MutationMethod.SwapMutation:
                    mutation = new SwapMutation(MutationProbability, 1);
                    break;
                case MutationMethod.TranslocationMutation:
                    mutation = new TranslocationMutation(MutationProbability, 6);
                    break;
                case MutationMethod.NonMutation:
                    mutation = null;
                    break;
            }

            return mutation;
        }
    }
}
