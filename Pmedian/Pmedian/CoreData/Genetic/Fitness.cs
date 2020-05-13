using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData;

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
            double badFitness = double.MaxValue;

            int n = cost.countVillage;
            int m = cost.countClinic + cost.countMedic;
            
            // TODO: Если деревня = 0 попадается то что то сделать , чтобы не попадалась

            for (int i = 0; i < n; i++)
            {
                int ichrom = 0;

                double tMedic = 0.0;
                double tAmbulance = 0.0;
                int sump = 0;
                for (int j = 0, c = ichrom; j < m; j++, c++)
                {
                    // суммы для проверки на органичения сверху или снизу числом
                    sump += chromosome.chromosomeArray[c];
                    tMedic += cost.costEdgeArray[i][j].timeMedic * chromosome.chromosomeArray[c];
                    tAmbulance += cost.costEdgeArray[i][j].timeAmbulance * chromosome.chromosomeArray[c];

                    //сумма функции
                    fitness += cost.costVertexArray[i] + (cost.costEdgeArray[i][j].roadKm * problemData.RoadCost +
                        cost.costEdgeArray[i][j].timeMedic + 
                        cost.costEdgeArray[i][j].timeAmbulance) * chromosome.chromosomeArray[c];
                    
                    ichrom++;
                }
                if (sump < problemData.P)
                    return badFitness;
                else if (tMedic > problemData.TimeMedic)
                    return badFitness;
                else if (tAmbulance > problemData.TimeAmbulance)
                    return badFitness;
            }

            return fitness;
        }
    }
}
