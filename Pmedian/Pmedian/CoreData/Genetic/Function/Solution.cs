
using Pmedian.CoreData.DataStruct;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace Pmedian.CoreData.Genetic.Function
{
    /// <summary>
    /// Статический класс проверки на сходимость ГА.
    /// </summary>
    public static class Solution
    {    
        public static bool isConverget(Chromosome bestCromosome, Chromosome worstChromosome)
        {
            double diff = Math.Abs(worstChromosome.fitness - bestCromosome.fitness);
            if (diff >= 0 && diff <= 1) return true;
            else return false;
        }

        /// <summary>
        /// Ищет среднюю приспособленность популяции.
        /// </summary>
        /// <param name="population">Популяция.</param>
        /// <returns>Средняя приспособленность.</returns>
        public static double MediumFitnessPopulation(Population population)
        {
            double sum = 0;
            foreach (var ch in population.populationList)
                sum += ch.fitness;

            double MFP = sum / population.SizePopulation;

            return MFP;
        }

        public static bool isAnswer(Chromosome bestChromosome, Cost cost, ProblemData problemData)
        { 
            int n = cost.countVillage;
            int m = cost.vertexCount;

            int chgencount = 0;
            for (int i = 0; i < n; i++)
            {
                double timeM = 0.0;
                double timeA = 0.0;
                int vertexMedian = 0;
                bool isNotEmptyCost = false;
                for (int j = 0, c = chgencount; j < m; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)                       
                        continue;
                        
                    timeM += cost.costEdgeArray[i][j].timeMedic * bestChromosome.chromosomeArray[c];
                    if (timeM > problemData.TimeMedic)
                        return false;

                        timeA += cost.costEdgeArray[i][j].timeAmbulance * bestChromosome.chromosomeArray[c];
                    if (timeA > problemData.TimeAmbulance)
                        return false;

                        vertexMedian += bestChromosome.chromosomeArray[c];

                    c++;
                    chgencount++;
                    isNotEmptyCost = true;
                }
                if (isNotEmptyCost && vertexMedian == 0)
                    return false;
                
            }

            return true;
        }
    }
}
