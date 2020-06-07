using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Selection
{
    /// <summary>
    /// Оператор селекции для алгоритма CHC, который отбирает N различных особей для скрещивания.
    /// </summary>
    static class SelectionForCHC 
    {
        /// <summary>
        /// Оператор селекции.
        /// </summary>
        /// <param name="populaion">Популяции, список хромосом.</param>
        /// <param name="N">Количество отбираемых особей.</param>
        /// <returns>Список хромосом.</returns>
       static public List<Chromosome> Selection(List<Chromosome> populaion, int N)
        {
            List<Chromosome> list = populaion;
            for (int i = 0; i < list.Count - 1; i++)
            {
                double min = list[i].fitness;
                int minId = i;
                for (int j = i + 1; j < list.Count; j++)
                {
                    double temp = list[j].fitness;
                    if (temp < min)
                    {
                        min = temp;
                        minId = j;
                    }
                }

                Chromosome tempChromosome = list[i];
                list.Insert(i, list[minId]);
                list.RemoveAt(i + 1);
                list.Insert(minId, tempChromosome);
                list.RemoveAt(minId + 1);

            }

            List<Chromosome> resultlist = list.GroupBy(ch => ch.chromosomeArray).Select(ch => ch.First()).ToList();
            

            resultlist.RemoveRange(N, resultlist.Count - N);
          

            return resultlist;
        }
    }
}
