using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Количество фельдшерских пунктов в графе.
        /// </summary>
        public int countMedic { get; set; }

        /// <summary>
        /// Количество пунктов скорой помощи в графе.
        /// </summary>
        public int countClinic { get; set; }

        public int vertexCount { get; private set; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе</param>
        public Cost(int countClinic, int countMedic, int countVillage)
        {
            this.countClinic = countClinic;
            this.countMedic = countMedic;
            this.countVillage = countVillage;
            this.vertexCount = countClinic + countMedic + countVillage;
            InitializeList();
        }

        /// <summary>
        /// Инициализация списка расходов. Создает полностью список расходов.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        private void InitializeList()
        {
            _costList = new List<List<int>>();
            costEdgeArray = new CostEdge[vertexCount][];
            costVertexArray = new double[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                _costList.Add(new List<int>());
                costVertexArray[i] = 0.0;
                costEdgeArray[i] = new CostEdge[vertexCount];                
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
            int countClinic = 0;
            int countMedic = 0;
            foreach (var vertex in vertices)
            {
                
                switch (vertex.Type)
                {
                    case VertexType.GroupeVillage:
                        countVillage++;
                        break;
                    case VertexType.GroupeClinic:
                        countClinic++;                        
                        break;
                    case VertexType.GroupeMedic:
                        countMedic++;                       
                        break;
                }
            }
            Console.WriteLine(countVillage);
            Console.WriteLine(countClinic);
            Console.WriteLine(countMedic);

            var cost = new Cost(countClinic, countMedic, countVillage);
            foreach (var vertex in vertices)
            {
                int target = vertices.IndexOf(vertex);
                Console.WriteLine($"vertex {target} type {vertex.Type} cost {vertex.vertexCost}");
                if (vertex.Type != VertexType.GroupeVillage)
                {
                    cost.AddCostVertex(target, vertex.vertexCost);                      
                }
            }

            foreach (var edge in graph.Edges)
            {
                int source = vertices.IndexOf(edge.Source);
                int target = vertices.IndexOf(edge.Target);
                
                if (edge.Source.Type != edge.Target.Type)
                {
                    if (edge.Source.Type == VertexType.GroupeVillage)
                    {
                        cost.AddEdge(source, target);
                        CostEdge costEdge = new CostEdge(edge.weigthR, edge.weigthA, edge.weigthM);
                        cost.AddCostEdge(source, target, costEdge);
                    }
                    else if (edge.Target.Type == VertexType.GroupeVillage)
                    {
                        cost.AddEdge(target, source);
                        CostEdge costEdge = new CostEdge(edge.weigthR, edge.weigthA, edge.weigthM);
                        cost.AddCostEdge(target, source, costEdge);
                    }
                }
            }


            cost.PrintCostList();
            cost.PrintCost();

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
                        Console.Write($" {costEdgeArray[i][j] }");
                        Console.Write($"({i}, {j}) ");
                        Console.Write($"{costEdgeArray[i][j].roadKm}; " +
                            $"{costEdgeArray[i][j].timeAmbulance}; " +
                            $"{costEdgeArray[i][j].timeMedic}");
                    }
                }
                Console.WriteLine();
            }
        }

        public void PrintCostList()
        {
            Console.WriteLine("print adjacency list COst");
            for (int i = 0; i < vertexCount; i++)
            {
                Console.Write($"{i} - ");
                foreach (int j in _costList[i])
                {
                    Console.Write(j);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }            
        }
    }
}
