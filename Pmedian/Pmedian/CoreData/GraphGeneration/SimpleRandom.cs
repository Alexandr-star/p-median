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
    /// Реализация простого алгоритма генерации случайного графа.
    /// </summary>
    public class SimpleRandom : IGraphGenerator
    {
        /// <summary>
        /// Количество вершин в графе.
        /// </summary>
        private int vertexCount;
        private int medCount;
        private int villCount;

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SimpleRandom(int vertexCount, int medCount, int villCount)
        {
            if (vertexCount < 3)
                throw new ArgumentOutOfRangeException(null, "Граф не может быть из двух вершин для этой задачи.");
            if (vertexCount <= medCount)
                throw new ArgumentOutOfRangeException(null, "Граф не может быть постоен. Слишком много мед. пунктов.");
            if (vertexCount <= villCount)
                throw new ArgumentOutOfRangeException(null, "Граф не может быть постоен. Слишком много пунктов.");
            if (vertexCount != villCount + medCount)
                throw new ArgumentOutOfRangeException(null, "Граф не может быть постоен. Неаерное количество вершин.");

            this.vertexCount = vertexCount;
            this.medCount = medCount;
            this.villCount = villCount;
        }

        /// <summary>
        /// Генерация нового случайного графа.
        /// </summary>
        /// <returns>Новый экземпляр сгенерированного графа.</returns>
        public MainGraph Generate()
        {
            var list = new AdjacencyList(vertexCount);

            // Создание связного графа с E = V - 1 
            for (int i = 1; i < vertexCount; i++)
                list.AddEdge(i, Utility.Rand.Next(0, i - 1));

            if (vertexCount < 3)
                return AdjacencyList.GenerateGraph(list, medCount, villCount);

            // Добавление новых ребер для создания циклов
            int new_edges = Utility.Rand.Next(0, vertexCount * (vertexCount - 1) / 2 - vertexCount);

            for (int e = 0; e < new_edges; e++)
            {
                int i = Utility.Rand.Next(0, vertexCount - 1);
                int j = Utility.Rand.Next(0, vertexCount - 1);

                if (i != j && !list.IsAdjacent(i, j))
                    list.AddEdge(i, j);
            }

            return AdjacencyList.GenerateGraph(list, medCount, villCount);
        }
    }
}
