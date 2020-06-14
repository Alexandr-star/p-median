using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;

namespace Pmedian.CoreData.DataStruct
{
    /// <summary>
    /// Класс, реализующий таблицу затрат, задаваемым матрицей.
    /// </summary>
    public class Cost
    {
        /// <summary>
        /// Матрица затрат.
        /// </summary>
        public CostEdge[][] costEdgeArray { get; private set; }

        private List<int> villageArray = new List<int>();
        /// <summary>
        /// Матрица связей медианной вершиные и не медианной вершины.
        /// Гарантирует, что каждая вершина деревни, прекреплена к вершине пункта
        /// </summary>
        public int[][] arrayX;

        private List<List<int>> _costList = new List<List<int>>();

        public List<List<int>> CostList
        {
            get => _costList;
        }

        /// <summary>
        /// Массив затрат на постройку пунктов. 
        /// </summary>
        public double[] costVertexArray { get; private set; }

        /// <summary>
        /// Количество деревень в графе.
        /// </summary>
        public int countVillage { get; set; } 

        public int vertexCount { get; private set; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе</param>
        public Cost(int vertexCount, int countVillage)
        {
            this.countVillage = countVillage;
            this.vertexCount = vertexCount;
            InitializeList();
        }

        /// <summary>
        /// Инициализация списка расходов. Создает полностью список расходов.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        private void InitializeList()
        {
            villageArray = new List<int>();
            _costList = new List<List<int>>();
            costEdgeArray = new CostEdge[vertexCount - countVillage][];
            costVertexArray = new double[vertexCount];
            arrayX = new int[vertexCount - countVillage][];
            for (int i = 0; i < vertexCount - countVillage; i++)
            {
                _costList.Add(new List<int>());
                costEdgeArray[i] = new CostEdge[vertexCount];
                arrayX[i] = new int[vertexCount];
                for (int j = 0; j < vertexCount; j++)
                {                   
                    costEdgeArray[i][j] = new CostEdge();
                }
            }                            
        }


        public static Cost CreateCostArray(MainGraph graph)
        {           
            var vertices = graph.Vertices.ToList();

            int countVillage = 0;
            foreach (var vertex in vertices)
            {
                if(VertexType.GroupeVillage == vertex.Type)
                    countVillage++;                   
            }
            Console.WriteLine(countVillage); 

            var cost = new Cost(graph.VertexCount, countVillage);
            foreach (var vertex in vertices)
            {
                int target = vertices.IndexOf(vertex);
                Console.WriteLine($"vertex {target} type {vertex.Type} cost {vertex.vertexCost}");
                if (vertex.Type != VertexType.GroupeVillage)
                {
                    cost.AddCostVertex(target, vertex.vertexCost);                      
                } 
                else
                {
                    cost.villageArray.Add(target);
                }
            }

            int[][] tempX = new int[cost.vertexCount][];
            CostEdge[][] tempCost = new CostEdge[cost.vertexCount][];
            for (int i = 0; i < cost.vertexCount; i++)
            {
                tempX[i] = new int[cost.vertexCount];
                tempCost[i] = new CostEdge[cost.vertexCount];
                for (int j = 0; j < cost.vertexCount; j++)
                    tempCost[i][j] = new CostEdge();
            }
            foreach (var edge in graph.Edges)
            {
                int source = vertices.IndexOf(edge.Source);
                int target = vertices.IndexOf(edge.Target);

                if (edge.Source.Type != edge.Target.Type)
                {
                    if (edge.Source.Type != VertexType.GroupeVillage && edge.Target.Type == VertexType.GroupeVillage)
                    {
                        CostEdge costEdge = new CostEdge(edge.Weight, edge.weigthA, edge.weigthM);
                        tempCost[source][target] = costEdge;
                        tempX[source][target] = 1;
                    }
                    else if (edge.Source.Type == VertexType.GroupeVillage && edge.Target.Type != VertexType.GroupeVillage)
                    {
                        CostEdge costEdge = new CostEdge(edge.Weight, edge.weigthA, edge.weigthM);
                        tempCost[target][source] = costEdge;
                        tempX[target][source] = 1;
                    }
                }
            }

            try
            {
                int index = 0;
                for (int i = 0; i < cost.vertexCount; i++)
                {                                                                   
                    if (cost.villageArray.Contains(i))
                        continue;
                    for (int j = 0; j < cost.vertexCount; j++)
                    {
                        cost.costEdgeArray[index][j] = tempCost[i][j];
                            cost.arrayX[index][j] = tempX[i][j];
                    }
                    index++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Ошибка в индексации");
            }

            cost.PrintCost();
            cost.PrintArray();
            return cost;
        }

        /// <summary>
        /// Добавить расход ребра в массив расходов ребер.
        /// </summary>
        /// <param name="source">Индекс деревни.</param>
        /// <param name="target">Индекс пункта.</param>
        /// <param name="costEdge">Расход ребра.</param>
        private void AddCostEdge(int indexVillage, int indexPoin, CostEdge costEdge)
        {
            costEdgeArray[indexVillage][indexPoin] = costEdge;
        }

        /// <summary>
        /// Добавить расход вершины в массив расходов вершин.
        /// </summary>
        /// <param name="index">Индекс вершины.</param>
        /// <param name="vertexCost">Расход вершины.</param>
        private void AddCostVertex(int index, double vertexCost)
        {
            costVertexArray[index] = vertexCost;
        }



        /// <summary>
        /// Добавить ребро в список смежности затрат.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void AddEdge(int source, int target)
        {
            _costList[source].Add(target);
        }

        private void PrintCost()
        {
            Console.WriteLine("print cost array edge");
            for (int i = 0; i < costEdgeArray.Length; i++)
            {
                Console.WriteLine($"vertex: {i}-{costVertexArray[i]}");
                for (int j = 0; j < costEdgeArray[i].Length; j++)
                {

                    if (costEdgeArray[i][j] != null)
                    {
                        Console.Write($"({i}, {j}) ");
                        Console.Write($"{costEdgeArray[i][j].roadKm}; " +
                            $"{costEdgeArray[i][j].timeAmbulance}; " +
                            $"{costEdgeArray[i][j].timeMedic}");
                    }
                }
                Console.WriteLine();
            }
        }

        private void PrintCost(CostEdge[][] arr)
        {
            Console.WriteLine("print cost array edge");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"vertex: {i}-{costVertexArray[i]}");
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.Write($"({i}, {j}) ");
                    Console.Write($"{arr[i][j].roadKm}; " +
                        $"{arr[i][j].timeAmbulance}; " +
                        $"{arr[i][j].timeMedic}");                    
                }
                Console.WriteLine();
            }
        }


        private void PrintArray()
        {
            Console.WriteLine("print array");
            for (int i = 0; i < arrayX.Length; i++)
            {
                Console.Write($"{i} - ");
                for(int j = 0; j < arrayX[i].Length; j++)
                {
                    Console.Write($"{arrayX[i][j]} ");
                }
                Console.WriteLine();
            }
        }

        private void PrintArray(int[][] arr)
        {
            Console.WriteLine("print array");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"{i} - ");
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.Write($"{arr[i][j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
