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

        public static double Function2(Cost cost, ProblemData problemData, Chromosome chromosome)
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
                    if (timeM > problemData.TimeMedic)
                    {
                        return double.MaxValue;
                    }

                    timeA += cost.costEdgeArray[i][j].timeAmbulance * chromosome.chromosomeArray[c];
                    if (timeA > problemData.TimeAmbulance)
                    {
                        return double.MaxValue;
                    }

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

                if (isNotEmptyCost && vertexMedian < problemData.P)
                    return double.MaxValue;
            }
            if (fitness == 0) return double.MaxValue;
            return fitness;
        }
    }
}
