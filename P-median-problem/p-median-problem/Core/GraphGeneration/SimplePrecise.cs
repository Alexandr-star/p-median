using ClusteringViz.Models;
using System;
using System.Collections.Generic;

namespace ClusteringViz.Core.GraphGeneration
{
    /// <summary>
    /// Реализация простого алгоритма генерации случайного графа с точным указанием количества ребер.
    /// </summary>
    public class SimplePrecise : IGraphGenerator
    {
        /// <summary>
        /// Количество вершин в графе.
        /// </summary>
        private int vertexCount;

        /// <summary>
        /// Количество ребер в графе.
        /// </summary>
        private int edgesCount;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        /// <param name="edgesCount">Количество ребер в графе.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SimplePrecise(int vertexCount, int edgesCount)
        {
            if (vertexCount < 1)
                throw new ArgumentOutOfRangeException(null, "Graph should contain at least one vertex.");

            if (edgesCount < vertexCount - 1)
                throw new ArgumentOutOfRangeException(null, "Not enough edges for connected graph generation.");

            if (edgesCount > vertexCount * (vertexCount - 1) / 2)
                throw new ArgumentOutOfRangeException(null, "Too many edges even for complete graph.");

            this.vertexCount = vertexCount;
            this.edgesCount = edgesCount;
        }

        /// <summary>
        /// Генерация нового случайного графа.
        /// </summary>
        /// <returns>Новый экземпляр сгенерированного графа.</returns>
        public MainGraph Generate()
        {
            var list = new AdjacencyList(vertexCount);
            
            // Создание списка ребер полного графа
            var edges = new List<Tuple<int, int>>();

            for (int i = 0; i < vertexCount - 1; i++)
                for (int j = i + 1; j < vertexCount; j++)
                    edges.Add(new Tuple<int, int>(i, j));

            // Создание связного графа с E = V - 1 
            for (int i = 1; i < vertexCount; i++)
                list.AddEdge(i, Utility.Rand.Next(0, i - 1));

            if (vertexCount < 3)
                return AdjacencyList.GenerateGraph(list);

            // Добавление еще ребер для создания циклов
            var additionalEdges = edgesCount - vertexCount + 1;

            while (additionalEdges > 0)
            {
                var edgeIndex = Utility.Rand.Next(0, edges.Count - 1);
                
                if (!list.IsAdjacent(edges[edgeIndex].Item1, edges[edgeIndex].Item2))
                {
                    list.AddEdge(edges[edgeIndex].Item1, edges[edgeIndex].Item2);
                    additionalEdges--;
                }

                edges.RemoveAt(edgeIndex);
            }

            return AdjacencyList.GenerateGraph(list);
        }
    }
}
