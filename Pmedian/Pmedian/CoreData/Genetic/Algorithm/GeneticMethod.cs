using Pmedian.CoreData.Genetic.Mutation;
using Pmedian.CoreData.Genetic.Selection;
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
        public static AbstractCrossover ChosenCrossoverMethod(CrossoverMethod crossoverMethod, double CrossoverProbability, int dots)
        {
            AbstractCrossover crossover = null;
            switch (crossoverMethod)
            {
                case CrossoverMethod.OneDot:
                    crossover = new OneDotCrossover(CrossoverProbability);
                    break;                
                case CrossoverMethod.NDot:
                    crossover = new NDotCrossover(dots, CrossoverProbability);
                    break;
            }

            return crossover;
        }

        public static AbstractMutation ChosenMutationMethod(MutationMethod mutationMethod, double MutationProbability, int pointMutation)
        {
            AbstractMutation mutation = null;
            switch (mutationMethod)
            {
                case MutationMethod.InversionMutation:
                    mutation = new InversionMutation(MutationProbability, pointMutation);
                    break;
                case MutationMethod.ReplaceMutation:
                    mutation = new ReplaceMutaion(MutationProbability, pointMutation);
                    break;
                case MutationMethod.SwapMutation:
                    mutation = new SwapMutation(MutationProbability, pointMutation);
                    break;
                case MutationMethod.TranslocationMutation:
                    mutation = new TranslocationMutation(MutationProbability, pointMutation);
                    break;
                case MutationMethod.NonMutation:
                    mutation = null;
                    break;
            }

            return mutation;
        }

        public static AbstractSelection ChosenSelectionMethod(SelectionMethod selectionMethod, int countTour, int countSelect)
        {
            AbstractSelection selection = null;
            switch (selectionMethod)
            {
                case SelectionMethod.Tournament:
                    selection = new TournamentSelection(countSelect, countTour);
                    break;
                case SelectionMethod.Proportion:
                    selection = new ProportionSelection(countSelect);
                    break;
            }
            return selection;
        }
    }
}
