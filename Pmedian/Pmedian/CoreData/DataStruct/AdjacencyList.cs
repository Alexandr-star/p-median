using Pmedian.CoreData.Genetic;
using Pmedian.Model;
using Pmedian.Model.Enums;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Pmedian.CoreData.DataStruct
{
    /// <summary>
    /// Класс, реализующий граф, задаваемый списком смежности.
    /// </summary>
    public class AdjacencyList
    {
        /// <summary>
        /// Список смежности графа.
        /// </summary>
        public List<List<int>> adjacencyList = new List<List<int>>();
     
        static public Cost cost { get; set; }
        
        /// <summary>
        /// Количество вершин в графе.
        /// </summary>
        public int VertexCount => adjacencyList.Count;

        /// <summary>
        /// Ориентированность графа.
        /// </summary>
        public bool IsDirected { get; private set; }

        
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        /// <param name="isDirected">Ориентированность графа.</param>
        public AdjacencyList(int vertexCount, bool isDirected = false)
        {
            InitializeList(vertexCount);
            IsDirected = isDirected;
        }

        /// <summary>
        /// Добавление нового ребра в граф.
        /// </summary>
        /// <param name="source">Исходная вершина.</param>
        /// <param name="target">Конечная вершина.</param>
        public void AddEdge(int source, int target)
        {
            adjacencyList[source].Add(target);

            if (!IsDirected)
                adjacencyList[target].Add(source);
        }
       
       
        /// <summary>
        /// Удаление ребра из графа.
        /// </summary>
        /// <param name="source">Исходная вершина.</param>
        /// <param name="target">Конечная вершина.</param>
        public void RemoveEdge(int source, int target)
        {
            adjacencyList[source].Remove(target);

            if (!IsDirected)
                adjacencyList[target].Remove(source);
        }

        /// <summary>
        /// Проверка, являются ли две вершины смежными.
        /// </summary>
        /// <param name="source">Исходная вершина.</param>
        /// <param name="target">Конечная вершина.</param>
        /// <returns>Истина, если вершины являются смежными.</returns>
        public bool IsAdjacent(int source, int target)
        {
            return adjacencyList[source].Contains(target);
        }
       

        /// <summary>
        /// Список смежных к исходной вершин.
        /// </summary>
        /// <param name="source">Исходная вершина.</param>
        /// <returns>Список смежных к исходной вершин.</returns>
        public List<int> GetAdjacent(int source)
        {
            return adjacencyList[source];
        }

        /// <summary>
        /// Инициализация списка смежности. Создает полностью несвязный граф.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        private void InitializeList(int vertexCount)
        {
            adjacencyList = new List<List<int>>();
            for (int i = 0; i < vertexCount; i++)
            {
                adjacencyList.Add(new List<int>());
            }                        
        }
                    
        /// <summary>
        /// Создание нового экземлпяра списка смежности на основе указанного графа.
        /// </summary>
        /// <param name="graph">Исходный граф.</param>
        /// <returns>Новый экземпляр графа, задаваемого списком смежности.</returns>
        public static AdjacencyList GenerateList(MainGraph graph)
        {
            var list = new AdjacencyList(graph.VertexCount);

            var vertices = graph.Vertices.ToList();
            
            foreach (var edge in graph.Edges)
            {
                int source = vertices.IndexOf(edge.Source);
                int target = vertices.IndexOf(edge.Target);
                list.AddEdge(source, target);
            }
            return list;
        }
       
        /// <summary>
        /// Создание нового экземлпяра графа на основе указанного списка смежности. 
        /// </summary>
        /// <param name="list">Исходный список смежности.</param>
        /// <returns>Новый экземпляр графа.</returns>
        public static MainGraph GenerateGraph(AdjacencyList list)
        {
            if (list == null)
                return null;
            MainGraph graph = new MainGraph();

            for (int i = 0; i < 60; i++)
                graph.AddVertex(new DataVertex());
            for (int i = 60; i < 100; i++)
            {
                graph.AddVertex(new DataVertex(VertexColor.Unmarked, VertexType.Unmarket, Utility.Rand.Next(999) + 1));
            }
            for (int i = 0; i < list.VertexCount; i++)
            {
                foreach (int j in list.GetAdjacent(i))
                {
                    if (list.IsDirected || !list.IsDirected && j > i)
                    {
                        var source = graph.Vertices.ElementAt(i);
                        var target = graph.Vertices.ElementAt(j);

                        graph.AddEdge(new DataEdge(source, target, Utility.Rand.Next(9) + 1));
                    }
                }
            }

            return graph;
        }              
    }
}
