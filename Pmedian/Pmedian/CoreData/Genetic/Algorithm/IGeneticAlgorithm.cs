using Pmedian.CoreData.DataStruct;
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
        /// Возвращает данные о работе алгоритма.
        /// </summary>
        /// <returns></returns>
        AlgorithmInfo GetAlgorithmInfo();

        /// <summary>
        /// Решение задачи p-медианы по указанному алгоритму.
        /// </summary>
        /// <param name="graph">Граф.</param>
        /// <param name="problemData">Параметры задачи</param>
        AdjacencyList GeneticAlgorithm(MainGraph graph, ProblemData problemData);
    }
}
