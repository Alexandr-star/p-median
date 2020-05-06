using Pmedian.CoreData.DataStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic
{
    class Chromosome
    {
        /// <summary>
        /// Длинна хромосомы.
        /// </summary>
        public int SizeChromosome { get; }

        /// <summary>
        /// Колическо деревьев штейнера в хромосоме.
        /// Считается, как количество деревень в графе.
        /// </summary>
        private int countSteinerTree;

        /// <summary>
        /// Список смежности графа.
        /// </summary>
        private AdjacencyList adjacencyList;

        /// <summary>
        /// Хромосома, представляющая из себя массив
        /// </summary>
        public int[] chromosomeArray { get; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="adjacencyList">Список смености графа.</param>
        public Chromosome(AdjacencyList adjacencyList)
        {
            this.adjacencyList = adjacencyList;
            this.countSteinerTree = adjacencyList.GetCountTypeVertex(0);
            this.SizeChromosome = countSteinerTree * 2;
            this.chromosomeArray = InitializeChromosome();
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="size">Размер хромосомы.</param>
        public Chromosome(int size)
        {
            this.SizeChromosome = size;
            this.chromosomeArray = new int[SizeChromosome];
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="chromosome">Массив int[] хромосомы.</param>
        public Chromosome(int[] arrayChromosome)
        {
            this.chromosomeArray = arrayChromosome;
            this.SizeChromosome = chromosomeArray.Length;
        }

        private int[] InitializeChromosome()
        {
            var list = adjacencyList.GetVillageList();
            int[] chromosome = new int[SizeChromosome];
            Random random = new Random();
            
            int jStep = 0;
            for (int i = 0; i < list.Count; i++)
            {
                int village = list.ElementAt(i);
                for (int j = jStep; j < 2; j++)
                {
                    List<int> listVillage = adjacencyList.GetAdjacent(village);
                    int count = listVillage.Count;
                    chromosome[j] = listVillage.ElementAt(random.Next(count));
                }
                jStep = jStep + 2;
            }

            return chromosome;
        }
    }
}
