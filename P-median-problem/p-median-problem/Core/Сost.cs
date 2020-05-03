using ClusteringViz.Models;
using ClusteringViz.Models.Enums;
using ClusteringViz.Windows;
using GraphX.Controls;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Windows.Media;

namespace ClusteringViz.Core
{
    /// <summary>
    /// Класс, реализующий таблицу затрат, задаваемым матрицей.
    /// </summary>
    public class Сost
    {
        /// <summary>
        /// Матрица затрат.
        /// </summary>
        private CostEdge[][] costEdgeArray;

        /// <summary>
        /// Массив затрат на постройку пунктов. 
        /// </summary>
        private double[] costVertexArray;

        /// <summary>
        /// Список смежности затрат.
        /// </summary>
        //private List<List<Cost>> costList = new List<List<Cost>>();
        

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе</param>
        public Сost(int vertexCount)
        {
            InitializeList(vertexCount);
        }

        /// <summary>
        /// Инициализация списка расходов. Создает полностью список расходов.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        private void InitializeList(int vertexCount)
        {
            costEdgeArray = new CostEdge[vertexCount][];
            costVertexArray = new double[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                costEdgeArray[i] = new CostEdge[vertexCount];
                costVertexArray[i] = 0.0;
                for (int j = 0; j < vertexCount; j++)
                {   
                    costEdgeArray[i][j] = null;
                }
            }
                
        }

        public Сost()
        {
            
        }

        public static Сost CreateCostArray(MainGraph graph)
        {
            var cost = new Сost(graph.VertexCount);

            var vertices = graph.Vertices.ToList();

            foreach (var vertex in graph.Vertices)
            {
                int target = vertices.IndexOf(vertex);
                cost.costVertexArray[target] = vertex.vertexCost;
            }

            foreach (var edge in graph.Edges)
            {
                int source = vertices.IndexOf(edge.Source);
                int target = vertices.IndexOf(edge.Target);

                var costEdge = new CostEdge(edge.weigthR, edge.weigthA, edge.weigthM);
                cost.costEdgeArray[source][target] = costEdge;
                cost.costEdgeArray[target][source] = costEdge;
            }

            cost.PrintCost();
            return cost;
        }
        

        private void PrintCost()
        {
            Console.WriteLine("print cost array");
            for (int i = 0; i < costEdgeArray.Length; i++)
            {
                Console.WriteLine($"vertex: {i}-{costVertexArray[i]}");
                for (int j = 0; j < costEdgeArray.Length; j++)
                {   
                   
                    Console.Write($"({i}, {j}) ");
                    if (costEdgeArray[i][j] != null)
                    {
                        Console.Write($"{costEdgeArray[i][j].roadCost}; " +
                            $"{costEdgeArray[i][j].timeAmbulance}; " +
                            $"{costEdgeArray[i][j].timeMedic}");
                    }
                }
                Console.WriteLine();
            }
        }

    }
}
