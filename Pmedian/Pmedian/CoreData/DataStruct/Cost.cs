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
        private int ROUND = 3;
        /// <summary>
        /// Матрица затрат.
        /// </summary>
        public CostEdge[][] costEdgeArray { get; private set; }

        public double SumAll { get; set; }
        public MainGraph mainGraph { get; set; }

        private List<int> villageArray = new List<int>();
        /// <summary>
        /// Матрица связей медианной вершиные и не медианной вершины.
        /// Гарантирует, что каждая вершина деревни, прекреплена к вершине пункта
        /// </summary>
        public int[][] arrayX;

        public List<int> unmarketVertex { get; set; }
     
        /// <summary>
        /// Массив затрат на постройку пунктов. 
        /// </summary>
        public List<double> costVertexArray { get; private set; }

        /// <summary>
        /// Количество деревень в графе.
        /// </summary>
        public int countVillage { get; set; } 

        public int vertexCount { get; private set; }

        public double midSpeedClinic;
        public double midSpeedMedic;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе</param>
        public Cost(int vertexCount, int countVillage, double midSpeedClinic, double midSpeedMedic)
        {
            this.midSpeedClinic = midSpeedClinic;
            this.midSpeedMedic = midSpeedMedic;
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
            unmarketVertex = new List<int>();
            costEdgeArray = new CostEdge[vertexCount - countVillage][];
            costVertexArray = new List<double>(vertexCount - countVillage);
            arrayX = new int[vertexCount - countVillage][];
            for (int i = 0; i < vertexCount - countVillage; i++)
            {
                costEdgeArray[i] = new CostEdge[vertexCount];
                arrayX[i] = new int[vertexCount];
                for (int j = 0; j < vertexCount; j++)
                {                   
                    costEdgeArray[i][j] = new CostEdge();
                }
            }                            
        }

        /// <summary>
        /// Созадет матрицу затрат на основе заданного графа.
        /// </summary>
        /// <param name="graph">Граф.</param>
        /// <param name="problemData">Параметры задачи.</param>
        /// <returns></returns>
       public static Cost GanerateCostArray(MainGraph graph, ProblemData problemData)
        {           
            var vertices = graph.Vertices.ToList();

            int countVillage = 0;
            foreach (var vertex in vertices)
            {
                if(VertexType.GroupeVillage == vertex.Type)
                    countVillage++;                   
            }
            Console.WriteLine(countVillage); 

            var cost = new Cost(graph.VertexCount, countVillage, problemData.MidSpeedMedic, problemData.MidSpeedAmbulance);
            cost.mainGraph = graph;
            foreach (var vertex in vertices)
            {
                int target = vertices.IndexOf(vertex);
                if (vertex.Type == VertexType.Unmarket)
                {
                    cost.unmarketVertex.Add(target);
                    cost.costVertexArray.Add(vertex.vertexCost);
                    cost.SumAll += vertex.vertexCost;
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
            // создали табицу весов
            foreach (var edge in graph.Edges)
            {
                int source = vertices.IndexOf(edge.Source);
                int target = vertices.IndexOf(edge.Target);
               
                CostEdge costEdge = new CostEdge(edge.Weight);
                costEdge.EmptyCost = false;
                costEdge.CostRoad = edge.Weight * problemData.RoadCost;               
                tempCost[source][target] = costEdge;
                tempCost[target][source] = costEdge;
            }
            cost.DDDDD(tempCost);
           
            return cost;
        }
      
        private void DDDDD(CostEdge[][] tempCost)
        {
            var vertices = mainGraph.Vertices.ToList();
            int index = 0;
            foreach (var vertex in vertices)
            {

                if (vertex.Type == VertexType.Unmarket)
                {
                    CreateCostArray(AlgD(vertex, tempCost[index], tempCost), index) ;
                    index++;                   
                }
            }
            
        }

        private void CreateCostArray(double[] vs, int index)
        {
            for (int i = 0; i < vertexCount; i++)
            {
                if (vs[i] == double.MaxValue)
                    continue;
                if (unmarketVertex.Contains(i))
                    continue;

                costEdgeArray[index][i].EmptyCost = false;
                costEdgeArray[index][i].roadKm = vs[i];
                SumAll += vs[i];
                costEdgeArray[index][i].timeС = Math.Round(vs[i] / midSpeedClinic, ROUND);
                costEdgeArray[index][i].timeM = Math.Round(vs[i] / midSpeedMedic, ROUND);
                arrayX[index][i] = 1;
            }
        }

        private double[] AlgD(DataVertex startVertex, CostEdge[] costEdges, CostEdge[][] costs)
        {
            var vertices = mainGraph.Vertices.ToList();
            double[] d = new double[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                d[i] = double.MaxValue;
                vertices[i].IsVisited = false;
            }
            d[vertices.IndexOf(startVertex)] = 0;
            double min = double.MaxValue;
            int minIndex = int.MaxValue;
            do
            {
                min = double.MaxValue;
                minIndex = int.MaxValue;

                for (int i = 0; i < vertexCount; i++)
                {

                    
                    // если вершину еще не обошли и вес меньше min
                    if (!vertices.ElementAt(i).IsVisited && d[i] < min)
                    {
                        min = d[i];
                        minIndex = i;
                    }
                }
                // добавляем найденный минимальный вес к текущему весу вершины
                // и сравниваем с текущим минимальным весом вершины
                if (minIndex < int.MaxValue)
                {
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (costs[minIndex][i].roadKm > 0)
                        {
                            double temp = min + costs[minIndex][i].roadKm;
                            if (temp < d[i])
                            {
                                d[i] = temp;
                            }
                        }
                    }
                    vertices.ElementAt(minIndex).IsVisited = true;
                }
            } while (minIndex < int.MaxValue);
            
            return d;
        }                                       
    }
}
