using Pmedian.Model;
using System;
using System.Linq;

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
        private CostEdge[][] costEdgeArray;

        /// <summary>
        /// Тестовая матрица.
        /// </summary>
        public int[][] TESTcostEdgeArray { get; private set; }

        public int countVillage { get; }

        public int countOtherPoint { get; }

        private Random random = new Random();


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
        public Cost(int vertexCount)
        {
            this.countVillage = 4;
            this.countOtherPoint = 9;
            InitializeTestCost();
            //InitializeList(vertexCount);
        }

        private void InitializeTestCost()
        {
            this.TESTcostEdgeArray = new int[countVillage][];
            for (int i = 0; i < countVillage; i++)
            {
                TESTcostEdgeArray[i] = new int[countOtherPoint];
                for (int j = 0; j < countOtherPoint; j++)
                {
                    TESTcostEdgeArray[i][j] = random.Next(100);
                }
            }
            PrintCost(TESTcostEdgeArray);
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


        public static Cost CreateCostArray(MainGraph graph)
        {
            var cost = new Cost(graph.VertexCount);

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

            //cost.PrintCost();
            return cost;
        }


        private void PrintCost(int[][] arrayCost)
        {
            Console.WriteLine("print cost array");
            for (int i = 0; i < arrayCost.Length; i++)
            {
                Console.WriteLine($"vertex: {i}-costVertexArray[i]");
                for (int j = 0; j < arrayCost[i].Length; j++)
                {
                    Console.Write($" {arrayCost[i][j] }");
                    //Console.Write($"({i}, {j}) ");
                    //if (arrayCost[i][j] != null)
                    //{
                        //Console.Write($"{arrayCost[i][j]}; " +
                        //    $"{arrayCost[i][j]}; " +
                        //    $"{arrayCost[i][j]}");
                    //}
                }
                Console.WriteLine();
            }
        }
    }
}
