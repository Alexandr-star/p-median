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
        private int SIZE_CROMOSOME = 0;

        /// <summary>
        /// Колическо деревьев штейнера в хромосоме.
        /// Считается, как количество деревень в графе.
        /// </summary>
        private int countSteinerTree = 0;

        /// <summary>
        /// Список смежности графа.
        /// </summary>
        private AdjacencyList adjacencyList;

        /// <summary>
        /// Хромосома, представляющая из себя массив
        /// </summary>
        private int[] chromosome;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="adjacencyList">Список смености графа.</param>
        public Chromosome(AdjacencyList adjacencyList)
        {
            this.adjacencyList = adjacencyList;
            this.countSteinerTree = adjacencyList.GetCountTypeVertex(0);
            this.SIZE_CROMOSOME = countSteinerTree * 2;
            this.chromosome = new int[SIZE_CROMOSOME];
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="size">Размер хромосомы.</param>
        public Chromosome(int size)
        {
            this.SIZE_CROMOSOME = size;
            this.chromosome = new int[SIZE_CROMOSOME];
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="chromosome">Массив int[] хромосомы.</param>
        public Chromosome(int[] arrayChromosome)
        {
            this.chromosome = arrayChromosome;
            this.SIZE_CROMOSOME = chromosome.Length;
        }

        public void InitializeChromosome()
        {
            var list = adjacencyList.GetVillageList();
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
           
        }

        public int[] GetArrayGensCromosome()
        {
            return chromosome;
        }

        public int Size()
        {
            return SIZE_CROMOSOME;
        }
    }
}
