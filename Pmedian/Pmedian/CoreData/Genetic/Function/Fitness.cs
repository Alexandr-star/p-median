using Pmedian.CoreData.DataStruct;
using System;

namespace Pmedian.CoreData.Genetic.Function
{
    /// <summary>
    /// Статический класс, описывающий функцию присособленности.
    /// </summary>
    public static class Fitness
    {
        /// <summary>
        /// Целефая функция.
        /// </summary>
        /// <param name="cost">Затраты.</param>
        /// <param name="problemData">Ограничивающие параметры.</param>
        /// <param name="chromosome">Хромосома.</param>
        /// <returns>Результат.</returns>
        public static double Function(Cost cost, ProblemData problemData, Chromosome chromosome)
        {
            double fitness = 0;
            int n = cost.countVillage;
            int m = cost.vertexCount;

            int chgencount = 0;
            int constant = 1;
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

                    timeM += cost.costEdgeArray[i][j].timeMedic * chromosome.chromosomeArray[c];
                    

                    timeA += cost.costEdgeArray[i][j].timeAmbulance * chromosome.chromosomeArray[c];
                    

                    vertexMedian += chromosome.chromosomeArray[c];

                    fitness += (
                        cost.costEdgeArray[i][j].timeMedic +
                        cost.costEdgeArray[i][j].timeAmbulance +
                        cost.costEdgeArray[i][j].roadKm * problemData.RoadCost +
                        cost.costVertexArray[j]
                        ) * chromosome.chromosomeArray[c];

                    c++;
                    chgencount++;
                    isNotEmptyCost = true;

                }
                if (timeM > vertexMedian * problemData.TimeMedic)
                {
                    constant++;
                }
                if (timeA > vertexMedian * problemData.TimeAmbulance)
                {
                    constant++;
                }
                if (isNotEmptyCost && vertexMedian < problemData.P)
                    return double.MaxValue;
            }
            if (fitness == 0) return double.MaxValue;
            return Math.Pow(fitness, constant);
        }

        public static double FunctionTrue(Cost cost, ProblemData problemData, Chromosome chromosome)
        {                                   
            double fitness = 0;
            int sumMedian = 0;
            int constant = 1;
            int[][] X = XMultiplicationChromosome(cost.arrayX, chromosome.chromosomeArray);



            for (int i = 0 ; i < chromosome.SizeChromosome; i++)
            {
                sumMedian += chromosome.chromosomeArray[i];
                
                for (int j = 0; j < cost.vertexCount; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)
                        continue;

                    if (!(X[i][j] <= chromosome.chromosomeArray[i]))
                        return double.MaxValue;
                    
                    if (cost.costEdgeArray[i][j].timeMedic > problemData.TimeMedic)
                    {
                        return double.MaxValue;
                        constant++;
                    }
                    if (cost.costEdgeArray[i][j].timeAmbulance > problemData.TimeAmbulance)
                    {
                        return double.MaxValue;
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
                return double.MaxValue;

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
                    return double.MaxValue;

                }
            }
           
            return fitness;
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
