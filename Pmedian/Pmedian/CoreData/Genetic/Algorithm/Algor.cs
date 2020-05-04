using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class Algor : IGeneticAlgorithm
    {
        /// <summary>
        /// Граф, задаваемый списком смежности вершин.
        /// </summary>
        private AdjacencyList adjacencyList;

        public Algor()
        {

        }

        public void GeneticAlgorithm(MainGraph graph)
        {
            // Инициализация основных структур
            adjacencyList = AdjacencyList.GenerateList(graph);
            Chromosome chromosome = new Chromosome(adjacencyList);
            chromosome.InitializeChromosome();
            Console.WriteLine("chromosome");
            for (int i = 0; i < chromosome.chromosome.Length; i++)
            {
                Console.Write($"  {chromosome.chromosome[i]}");
            }
        }
    }
}
