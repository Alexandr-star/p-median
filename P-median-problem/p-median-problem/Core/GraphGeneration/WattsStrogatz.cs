using p_median_problem.Models;
using System;

namespace p_median_problem.Core.GraphGeneration
{
    /// <summary>
    /// Реализация генерации случайного графа по модели Watt–Strogatz.
    /// </summary>
    public class WattsStrogatz : IGraphGenerator
    {
        /// <summary>
        /// Количество вершин в графе.
        /// </summary>
        private int vertexCount;

        /// <summary>
        /// Средняя степень вершины графа.
        /// </summary>
        private int meanDegree;

        /// <summary>
        /// Вероятность перестроения ребра на втором этапе алгоритма.
        /// </summary>
        private double beta;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        /// <param name="meanDegree">Средняя степень вершины.</param>
        /// <param name="beta">Вероятность перестроения ребра на втором этапе алгоритма.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public WattsStrogatz(int vertexCount, int meanDegree, double beta)
        {
            if (vertexCount < 1)
                throw new ArgumentOutOfRangeException(null, "Graph should contain at least one vertex.");

            if (meanDegree < 2 || meanDegree % 2 != 0)
                throw new ArgumentOutOfRangeException(null, "Mean vertex degree should be even and it's value should be not less than 2.");

            if (beta < 0 || beta > 1)
                throw new ArgumentOutOfRangeException(null, "Edge rewrite probability value should be in range from 0 to 1.");

            this.vertexCount = vertexCount;
            this.meanDegree = meanDegree;
            this.beta = beta;
        }
        
        /// <summary>
        /// Генерация нового случайного графа.
        /// </summary>
        /// <returns>Новый экземпляр сгенерированного графа.</returns>
        public MainGraph Generate()
        {
            var list = new AdjacencyList(vertexCount);

            // Генерация простого кольца
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 1; j <= meanDegree / 2; j++)
                {
                    list.AddEdge(i, (i + j) % vertexCount);
                }
            }

            // Перестроение ребер, соединяющих каждую вершину графа с ее K/2 правыми соседями, с вероятностью beta
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 1; j <= meanDegree / 2; j++)
                {
                    if (Utility.Rand.NextDouble() < beta)
                    {
                        var k = Utility.Rand.Next(0, vertexCount - 1);
            
                        if (k != i && !list.IsAdjacent(i, k))
                        {
                            list.AddEdge(i, k);
                            list.RemoveEdge(i, (i + j) % vertexCount);
                        }
                    }
                }
            }
            
            return AdjacencyList.GenerateGraph(list);
        }
    }
}
