using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pmedian.CoreData.GraphGeneration
{
    public class PmedianGraphGenerate : IGraphGenerator
    {
        private int vertexVillage;
        private int vertexClinic;
        private int vertexAmbulator;
        private int vertexCount;

        public PmedianGraphGenerate(int vertexVillage, int vertexClinic, int vertexAmbulator)
        {
            this.vertexVillage = vertexVillage;
            this.vertexClinic = vertexClinic;
            this.vertexAmbulator = vertexAmbulator;
            this.vertexCount = vertexClinic + vertexVillage + vertexAmbulator;
        }
       
        public MainGraph Generate()
        {
            var list = new AdjacencyList(vertexCount);
            int size = vertexClinic + vertexAmbulator;

            /*for (int i = 0; i < vertexVillage; i++)
            {
                for (int j = vertexVillage; j < vertexCount; j++)
                    list.AddEdge(i, j);
            }*/
            MainGraph graph = new MainGraph();
            for (int i = 0; i < vertexVillage; i++)
                graph.AddVertex(new DataVertex(VertexColor.GroupeVillage, VertexType.GroupeVillage, Utility.Rand.Next(50, 10000)));
            for (int i = vertexVillage; i < vertexVillage + vertexClinic; i++)
                graph.AddVertex(new DataVertex(VertexColor.GroupeClinic, VertexType.GroupeClinic, Utility.Rand.Next(10, 9999) + Math.Round(Utility.Rand.NextDouble(), 3)));
            for (int i = vertexVillage + vertexClinic; i < vertexVillage + vertexClinic + vertexAmbulator; i++)
                graph.AddVertex(new DataVertex(VertexColor.GroupeMedic, VertexType.GroupeMedic, Utility.Rand.Next(10, 9999) + Math.Round(Utility.Rand.NextDouble(), 3)));

            for (int v = 0; v < vertexVillage; v++)
            {
                for (int c = vertexVillage; c < vertexCount ; c++)
                {

                    var source = graph.Vertices.ElementAt(v);
                    var target = graph.Vertices.ElementAt(c);
                    graph.AddEdge(new DataEdge(source, target,
                        Utility.Rand.Next(1, 9999) + Math.Round(Utility.Rand.NextDouble(), 3)));
                }
                
            }


            return graph;
        }
    }        
}
