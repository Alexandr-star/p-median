using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData;
using System.Windows.Documents;
using System.Collections.Generic;
using System;

namespace Pmedian.CoreData.Genetic
{
    /// <summary>
    /// Статический класс, описывающий функцию присособленности.
    /// </summary>
    public static class Fitness
    {
        /// <summary>
        /// Функция приспособленности.
        /// </summary>
        /// <param name="cost">Затраты.</param>
        /// <param name="problemData">Ограничивающие параметры.</param>
        /// <param name="chromosome">Хромосома.</param>
        /// <returns>Приспособленность хромосомы.</returns>
        public static double Function(Cost cost, ProblemData problemData, Chromosome chromosome)
        {
            double fitness = 0;
            int n = cost.countVillage;
            int m = cost.vertexCount;

            int chgencount = 0;
            for (int i = 0;  i < n; i++)
            {
                double timeM = 0.0;
                double timeA = 0.0;
                int vertexMedian = 0;
                for (int j = 0, c = chgencount; j < m; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)
                        continue;

                    timeM += cost.costEdgeArray[i][j].timeMedic;
                    if (timeM > problemData.TimeMedic)
                    {
                        Console.WriteLine();
                        return double.MaxValue;
                    }

                    timeA += cost.costEdgeArray[i][j].timeAmbulance;
                    if (timeA > problemData.TimeAmbulance)
                    {
                        Console.WriteLine();
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

                }
                Console.Write(vertexMedian);
                Console.Write(" ");
                if (vertexMedian < problemData.P)
                {
                    Console.WriteLine();
                    return double.MaxValue;
                }
            }
            Console.WriteLine();

            return fitness;
        }
    }
}
