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
                        constant++;
                    }                    
                        
                    
                    if (cost.costEdgeArray[i][j].timeM > problemData.MedicTime)
                    {

                        constant++;
                    }
                    if (cost.costEdgeArray[i][j].timeС > problemData.AmbulanceTime)
                    {

                        constant++;

                    }

                    fitness += (
                        cost.costEdgeArray[i][j].roadKm * problemData.RoadCost *
                        cost.costVertexArray[i]
                        ) * X[i][j];
                }
                
            }
            if (sumMedian == 0)
                return double.MaxValue;
            if (sumMedian != problemData.P)
            {
                return double.MaxValue;

            }

            return fitness * (constant );
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
