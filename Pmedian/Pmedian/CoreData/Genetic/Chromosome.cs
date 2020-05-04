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
        /// <param name="adjacencyList"></param>
        public Chromosome(AdjacencyList adjacencyList)
        {
            this.adjacencyList = adjacencyList;
            this.countSteinerTree = adjacencyList.GetCountTypeVertex(0);
            this.SIZE_CROMOSOME = adjacencyList.GetCountTypeVertex(1) + adjacencyList.GetCountTypeVertex(2);
        }

        public Chromosome(int size)
        {
            this.SIZE_CROMOSOME = size;
        }

        private void InitializeChromosome()
        {
            var list = adjacencyList.GetVillageList();
            chromosome = new int[SIZE_CROMOSOME];
            Random random = new Random();
            int jStep = 0;
            for (int i = 0; i < list.Count; i++)
            {
                int village = list.ElementAt(i);
                for (int j = jStep; j < SIZE_CROMOSOME / countSteinerTree; j++)
                {
                    List<int> count = adjacencyList.GetAdjacent(village);
                }
                jStep++;
            }
           

        }
    }
}
