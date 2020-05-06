using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class GenitorGA : AbstractGeneticAlgorithm
    {
        /// <summary>
        /// Граф, задаваемый списком смежности вершин.
        /// </summary>
        private AdjacencyList adjacencyList;

        /// <summary>
        /// Размер популяции.
        /// </summary>
        private int PopulationSize;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="IterationSize">Количество итераций генетического алгоритма.</param>
        public GenitorGA(int IterationSize, int PopulationSize) : base(IterationSize)
        {
            this.PopulationSize = PopulationSize;
        }

        /// <summary>
        /// Реализация генетического алгоритма "Genitoe".
        /// </summary>
        /// <param name="graph">Граф.</param>
        public override void GeneticAlgorithm(MainGraph graph)
        {
            //Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            Chromosome[] startPopulation = new Population(PopulationSize, adjacencyList).populationArray;
        }
    }
}
