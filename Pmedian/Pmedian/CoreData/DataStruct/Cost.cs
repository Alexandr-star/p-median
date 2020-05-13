using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
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

        

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе</param>
        public Cost(int countClinic, int countMedic, int countVillage)
        {
            this.countClinic = countClinic;
            this.countMedic = countMedic;
            this.countVillage = countVillage;
            InitializeList();
        }

        /// <summary>
        /// Инициализация списка расходов. Создает полностью список расходов.
        /// </summary>
        /// <param name="vertexCount">Количество вершин в графе.</param>
        private void InitializeList()
        {
            costEdgeArray = new CostEdge[countVillage][];
            costVertexArray = new double[countVillage + countMedic + countClinic];
            for (int i = 0; i < countVillage; i++)
            {
                costEdgeArray[i] = new CostEdge[countVillage + countMedic + countClinic];                
                for (int j = 0; j < countVillage + countMedic + countClinic; j++)
                {
                    costVertexArray[j] = 0.0;
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
            
            var cost = new Cost(countClinic, countMedic, countVillage);
            foreach (var vertex in vertices)
            {
                int target = vertices.IndexOf(vertex);
                Console.WriteLine($"vertex {target} type {vertex.Type} cost {vertex.vertexCost}");
                if (vertex.Type != VertexType.GroupeVillage)
                {
                    cost.costVertexArray[target] = vertex.vertexCost;                      
                }
            }
            
            // TODO: Исправить добовление свяаных деревень в массив.
            foreach (var vertex in vertices)
            {
                if (vertex.Type == VertexType.GroupeVillage)
                {
                    int targetVerex = vertices.IndexOf(vertex);
                    foreach (var edge in graph.Edges)
                    {
                        int source = vertices.IndexOf(edge.Source);
                        int target = vertices.IndexOf(edge.Target);
                        var costEdge = new CostEdge(edge.weigthR, edge.weigthA, edge.weigthM);
                        if (source == targetVerex)
                        {                           
                            cost.costEdgeArray[targetVerex][target] = costEdge;
                        } else if (target == targetVerex)
                        {
                            cost.costEdgeArray[targetVerex][source] = costEdge;
                        }                                              
                    }
                }               
            }

            
            

            cost.PrintCost();

            return cost;
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
    }
}
