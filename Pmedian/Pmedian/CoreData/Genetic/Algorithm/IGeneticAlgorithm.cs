using Pmedian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    /// <summary>
    /// Интерфейс для генетических алгоритмов поиска решения.
    /// </summary>
    public interface IGeneticAlgorithm
    {
        /// <summary>
        /// Решение задачи p-медианы по указанному алгоритму.
        /// </summary>
        /// <param name="graph">Граф.</param>
        void GeneticAlgorithm(MainGraph graph);
    }
}
