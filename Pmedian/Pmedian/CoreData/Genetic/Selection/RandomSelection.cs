using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Selection
{
    /// <summary>
    /// Оператор селекции, который отбирает двух случайных родителей.
    /// </summary>
    class RandomSelection
    {
        public RandomSelection() { }
        /// <summary>
        /// Оператор селекции.
        /// </summary>
        /// <param name="population">Список особей (популяция).</param>
        /// <returns>Списокой отобраныйх особей (2 штуки).</returns>
        public List<Chromosome> Selection(List<Chromosome> population)
        {
            List<Chromosome> selectionList = new List<Chromosome>();
            int indexFP = 0;
            int indexSP = 0;
            while (indexFP == indexSP)
            {
                indexFP = Utility.Rand.Next(population.Count);
                indexSP = Utility.Rand.Next(population.Count);
            }
            selectionList.Add(population.ElementAt(indexFP));
            selectionList.Add(population.ElementAt(indexSP));

            return selectionList;
        }

        public int[] indexTwoParant { get; set; }
        public List<Chromosome> Selection(List<Chromosome> population, int[] indexParant)
        {
            List<Chromosome> selectionList = new List<Chromosome>();
            int indexFP = 0;
            int indexSP = 0;
            while (indexFP == indexSP)
            {
                indexFP = Utility.Rand.Next(population.Count);
                indexSP = Utility.Rand.Next(population.Count);
            }
            indexTwoParant = new int[2];
            indexTwoParant[0] = indexParant[indexFP];
            indexTwoParant[1] = indexParant[indexSP];
            selectionList.Add(population.ElementAt(indexFP));
            selectionList.Add(population.ElementAt(indexSP));

            return selectionList;
        }
    }
}
