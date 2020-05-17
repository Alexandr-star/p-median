using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Selection
{
    /// <summary>
    /// Селективное давление.
    /// </summary>
    public static class SelectionPressure
    {
        /// <summary>
        /// Функция селективного давления.
        /// </summary>
        /// <param name="bestChromosome">Наилучшая хромосома.</param>
        /// <param name="population">Популяция.</param>
        /// <returns></returns>
        public static double S(Chromosome bestChromosome, Population population)
        {
            return bestChromosome.fitness / Average(population);
        }

        /// <summary>
        /// Средняя оценка приспособленности.
        /// </summary>
        /// <param name="population">Популяция.</param>
        /// <returns>Средняя оценка приспособленности по всей популяции.</returns>
        private static double Average(Population population)
        {
            double sum = 0.0;
            for (int i = 0; i < population.SizePopulation; i++)
            {
                sum += population.populationList[i].fitness;
            }

            return sum / population.SizeChromosome;
        }
    }
}
