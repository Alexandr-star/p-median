﻿using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Pmedian.CoreData.DataStruct
{
    /// <summary>
    /// Класс, реализующий граф, задаваемый списком смежности.
    /// </summary>
    public class AdjacencyList
    {
        /// <summary>
        /// Список смежности.
        /// </summary>
        private List<List<int>> adjacencyList = new List<List<int>>();

        private List<List<int>> typeListVertex = new List<List<int>>();

        /// <summary>
        /// Количество вершин в графе.
        /// </summary>
        public int VertexCount => adjacencyList.Count;

        /// <summary>
        /// Максимальное количество вершин в списке смежности.
        /// </summary>
        /// TODO: ошибка вылетает, надо исправить
        public int VertexCountInMaxList => adjacencyList.Max().Count;

        /// <summary>
        /// Связность графа.
        /// </summary>
        public bool IsConnected => CheckConnectivity();

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
        /// Возращает список с деревнями
        /// </summary>
        /// <returns></returns>
        public List<int> GetVillageList()
        {
            return typeListVertex[0];
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
            typeListVertex = new List<List<int>>();
            for (int i = 0; i < vertexCount; i++)
                adjacencyList.Add(new List<int>());
            for (int i = 0; i < 3; i++)
                typeListVertex.Add(new List<int>());
        }

        /// <summary>
        /// Добавить вершину в список вершинын распределенных по типу.
        /// </summary>
        /// <param name="type">Тип вершины.</param>
        /// <param name="vertex">Вершина.</param>
        private void AddVertexInTypeList(int type, int vertex)
        {
            typeListVertex[type].Add(vertex);
        }

        /// <summary>
        /// Возвратить количество вершин определенного типа.
        /// </summary>
        /// <param name="type">Тип вершины.</param>
        /// <returns>Количество вершин.</returns>
        public int GetCountTypeVertex(int type)
        {
            if (type == 0)
                return typeListVertex.ElementAt(type).Count;
            else if (type == 1)
                return typeListVertex.ElementAt(type).Count;
            else if (type == 2)
                return typeListVertex.ElementAt(type).Count;
            else
                return 0;
        }

        private int MaxCountListInAdj()
        {
            List<int> countList = new List<int>();
            foreach(var list in adjacencyList)
            {
                countList.Add(list.Count);
            }
            return countList.Max();
        }

        /// <summary>
        /// Проверка связности графа. Истинна, если граф является связным.
        /// </summary>
        /// <returns>Истина, если граф является связным.</returns>
        private bool CheckConnectivity()
        {
            var nodes = new List<int>(VertexCount);
            var visited = Enumerable.Repeat(false, VertexCount).ToArray();

            nodes.Add(0);
            while (nodes.Count > 0)
            {
                var curr = nodes[0]; nodes.RemoveAt(0);

                foreach (var val in GetAdjacent(curr))
                    if (visited[val] == false)
                        nodes.Add(val);

                visited[curr] = true;
            }

            foreach (var val in visited)
                if (val == false)
                    return false;

            return true;
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


            foreach (var vertex in vertices)
            {
                if (vertex.Type == VertexType.GroupeVillage)               
                    list.AddVertexInTypeList(0, vertices.IndexOf(vertex));               
                if (vertex.Type == VertexType.GroupeClinic)                 
                    list.AddVertexInTypeList(1, vertices.IndexOf(vertex));             
                if (vertex.Type == VertexType.GroupeMedic) 
                    list.AddVertexInTypeList(2, vertices.IndexOf(vertex));
            }

            foreach (var edge in graph.Edges)
            {
                int source = vertices.IndexOf(edge.Source);
                int target = vertices.IndexOf(edge.Target);

                list.AddEdge(source, target);
            }
            PrintGraph(list);
            return list;
        }


        public static AdjacencyList GenerateList(int[] chromosome, int villageCount, int otherPoint)
        {
            var list = new AdjacencyList(villageCount + otherPoint);

            int ch = 0;
            for (int i = 0; i < villageCount; i++)
            {
                for (int j = 0, c = ch; j < otherPoint; j++, c++)
                {
                    if (chromosome[j] == 1) 
                        list.AddEdge(i, j);
                    ch += c + 1;
                }
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
            MainGraph graph = new MainGraph();

            for (int i = 0; i < list.VertexCount; i++)
                graph.AddVertex(new DataVertex());

            for (int i = 0; i < list.VertexCount; i++)
            {
                foreach (int j in list.GetAdjacent(i))
                {
                    if (list.IsDirected || !list.IsDirected && j > i)
                    {
                        var source = graph.Vertices.ElementAt(i);
                        var target = graph.Vertices.ElementAt(j);

                        graph.AddEdge(new DataEdge(source, target));
                    }
                }
            }

            return graph;
        }

        public static void PrintGraph(AdjacencyList list)
        {
            Console.WriteLine("print adjacency list");
            for (int i = 0; i < list.VertexCount; i++)
            {
                Console.WriteLine(i);
                foreach (int j in list.GetAdjacent(i))
                {
                    Console.Write(j);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
