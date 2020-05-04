using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.GraphGeneration
{
    /// <summary>
    /// Реализация генерации случайного графа по модели Erdos-Renyi.
    /// </summary>
    public class ErdosRenyi : IGraphGenerator
    {
        /// <summary>
        /// Количество вершин в графе.
        /// </summary>
        private int vertexCount;

        /// <summary>
        /// Вероятность добавления ребра между двумя случайными вершинами.
        /// </summary>
        private double probability;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        /// <param name="probability">Вероятность добавления ребра между двумя случайными вершинами.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ErdosRenyi(int vertexCount, double probability)
        {
            if (vertexCount < 1)
                throw new ArgumentOutOfRangeException(null, "Graph should contain at least one vertex.");

            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(null, "Edge generation probability value should be in range from 0 to 1.");

            this.vertexCount = vertexCount;
            this.probability = probability;
        }

        /// <summary>
        /// Генерация нового случайного графа.
        /// </summary>
        /// <returns>Новый экземпляр сгенерированного графа.</returns>
        public MainGraph Generate()
        {
            var list = new AdjacencyList(vertexCount);

            // Генерация графа по модели Erdos-Renyi
            for (int i = 0; i < vertexCount - 1; i++)
                for (int j = i; j < vertexCount; j++)
                    if (Utility.Rand.NextDouble() < probability)
                        list.AddEdge(i, j);

            return AdjacencyList.GenerateGraph(list);
        }
    }
}
