using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    abstract class AbstractGeneticAlgorithm : IGeneticAlgorithm
    {
        /// <summary>
        /// Число итераций генетического алгоритма.
        /// </summary>
        public int IterateSize { get; set; }

        /// <summary>
        /// Конструктор с параметрами абстрактного класса.
        /// </summary>
        /// <param name="adjacencylist">Список смежности графа.</param>
        /// <param name="population">Популяция, список хромосом.</param>
        public AbstractGeneticAlgorithm(int iterate)
        {
            this.IterateSize = iterate;
        }

        /// <summary>
        /// Абстрактный метод генетического алгоритма
        /// </summary>
        /// <param name="graph"></param>
        public abstract void GeneticAlgorithm(MainGraph graph);
    }
}
