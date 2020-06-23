using Pmedian.CoreData.DataStruct;
using System;

namespace Pmedian.CoreData.Genetic.Function
{
    /// <summary>
    /// Статический класс, описывающий функцию присособленности.
    /// </summary>
    public static class Fitness
    {
        public static double FunctionTrue(Cost cost, ProblemData problemData, Chromosome chromosome)
        {                                   
            double fitness = 0;
            int sumMedian = 0;
            int constant = 1;
            int xcrit = 0;
            int t1crit = 0;
            int t2crit = 0;
            int pcrit = 0;
            int[][] X = XMultiplicationChromosome(cost.arrayX, chromosome.chromosomeArray);

            for (int i = 0 ; i < chromosome.SizeChromosome; i++)
            {
                sumMedian += chromosome.chromosomeArray[i];
                
                for (int j = 0; j < cost.vertexCount; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)
                        continue;

                    if (!(X[i][j] <= chromosome.chromosomeArray[i]))
                    {
                        return 1.0 / cost.SumAll;
                    }                    
                        
                    
                    if (cost.costEdgeArray[i][j].timeM > problemData.MedicTime)
                    {

                        return 1.0 / cost.SumAll;
                    }
                    if (cost.costEdgeArray[i][j].timeС > problemData.AmbulanceTime)
                    {

                        return 1.0 / cost.SumAll;

                    }

                    fitness += (
                        (cost.costEdgeArray[i][j].roadKm * problemData.RoadCost) * X[i][j]);
                }
                fitness += cost.costVertexArray[i] * chromosome.chromosomeArray[i];
        }
            if (sumMedian == 0)
                return 1.0/ cost.SumAll;
            if (sumMedian != problemData.P)
            {
                return 1.0 / cost.SumAll;

            }

            return 1.0 / fitness;
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
