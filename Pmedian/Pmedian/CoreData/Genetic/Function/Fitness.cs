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
                for (int j = 0, c = chgencount; j < m; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)
                        continue;

                    timeM += cost.costEdgeArray[i][j].timeMedic;
                    if (timeM > problemData.TimeMedic)
                    {
                       Console.WriteLine();
                        constant++;
                    }

                    timeA += cost.costEdgeArray[i][j].timeAmbulance;
                    if (timeA > problemData.TimeAmbulance)
                    {
                        Console.WriteLine();
                        constant++;
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
                if (vertexMedian < problemData.P)
                    constant++;
                Console.Write(" ");
            }
            Console.WriteLine();

            return Math.Pow(fitness, constant);
        }
    }
}
