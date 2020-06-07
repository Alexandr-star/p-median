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
     static class RandomSelection
    {
        /// <summary>
        /// Оператор селекции.
        /// </summary>
        /// <param name="population">Список особей (популяция).</param>
        /// <returns>Списокой отобраныйх особей (2 штуки).</returns>
        static public List<Chromosome> Selection(List<Chromosome> population)
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
    }
}
