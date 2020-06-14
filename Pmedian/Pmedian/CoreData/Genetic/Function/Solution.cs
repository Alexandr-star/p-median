
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
                        
                    timeM = cost.costEdgeArray[i][j].timeMedic * bestChromosome.chromosomeArray[c];
                    if (timeM > problemData.TimeMedic)
                    {
                        
                        return false;
                    }
                    timeA = cost.costEdgeArray[i][j].timeAmbulance * bestChromosome.chromosomeArray[c];
                    if (timeA > problemData.TimeAmbulance)
                        return false;

                    vertexMedian += bestChromosome.chromosomeArray[c];

                    c++;
                    chgencount++;
                    isNotEmptyCost = true;
                }
                if (isNotEmptyCost && vertexMedian < problemData.P)
                    return false;
                
            }

            return true;
        }
       
        public static bool isAnswerTrue(Chromosome chromosome, Cost cost, ProblemData problemData)
        {
            double fitness = 0;
            int sumMedian = 0;
            int constant = 1;
            int[][] X = XMultiplicationChromosome(cost.arrayX, chromosome.chromosomeArray);

            for (int i = 0; i < chromosome.SizeChromosome; i++)
            {
                sumMedian += chromosome.chromosomeArray[i];

                for (int j = 0; j < cost.vertexCount; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)
                        continue;

                    if (X[i][j] <= chromosome.chromosomeArray[i])
                    {
                    }
                    else
                    {
                        return false;
                    }
                    if (cost.costEdgeArray[i][j].timeMedic > problemData.TimeMedic)
                    {
                        return false;
                        constant++;
                    }
                    if (cost.costEdgeArray[i][j].timeAmbulance > problemData.TimeAmbulance)
                    {
                        return false;
                        constant++;
                    }

                    fitness += (
                        cost.costEdgeArray[i][j].timeMedic +
                        cost.costEdgeArray[i][j].timeAmbulance +
                        cost.costEdgeArray[i][j].roadKm * problemData.RoadCost +
                        cost.costVertexArray[j]
                        ) * X[i][j];
                }

            }
            if (sumMedian == problemData.P)
            {
            }
            else
            {
                return false;

            }
            for (int j = 0; j < cost.vertexCount; j++)
            {
                int sumx = 0;

                for (int i = 0; i < chromosome.SizeChromosome; i++)
                {
                    sumx += X[i][j];
                }
                if (sumx == 1)
                {
                }
                else if (sumx != 0)
                {
                    return false;

                }
            }

            return true;
        }

        private static int[][] XMultiplicationChromosome(int[][] arrayX, int[] chromosomeArray)
        {

            int[][] array = new int[arrayX.Length][];

            for (int i = 0; i < arrayX.Length; i++)
            {
                array[i] = new int[arrayX[i].Length];
                for (int j = 0; j < arrayX[i].Length; j++)
                {
                    array[i][j] = arrayX[i][j] * chromosomeArray[i];
                }
            }

            return array;
        }
    }
}
