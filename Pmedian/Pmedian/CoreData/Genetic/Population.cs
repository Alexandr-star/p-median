using Pmedian.CoreData.DataStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic
{
    /// <summary>
    /// Класс реализующий популяцию хромосом.
    /// </summary>
    class Population
    {
        /// <summary>
        /// Размер популяции.
        /// </summary>
        public int SizePopulation { get; set; }

        /// <summary>
        /// Популяция, предаставленная массивом хромомсом.
        /// </summary>
        public Chromosome[] populationArray { get; set; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="sizePopulation">Размер популяции.</param>
        /// <param name="adjacencyList">Список смежности графа.</param>
        public Population(int sizePopulation, AdjacencyList adjacencyList)
        {
            this.SizePopulation = sizePopulation;
            this.populationArray = initializePopulation(adjacencyList);
        }

        private Chromosome[] initializePopulation(AdjacencyList adjacencyList)
        {
            Chromosome[] array = new Chromosome[SizePopulation];
            
            for (int i = 0; i < SizePopulation; i++)
            {
                array[i] = new Chromosome(adjacencyList);
            }

            return array;
        }
    }
}
