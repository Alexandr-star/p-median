using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Reduction
{
    public static class EliteReductionForCHC
    {
        public static List<Chromosome> Reduction(List<Chromosome> parentList, List<Chromosome> childList, int PopulationSize)
        {
            List<Chromosome> list = new List<Chromosome>();

            list.AddRange(parentList);
            list.AddRange(childList);
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

            List<Chromosome> newPopulationList = list.GroupBy(ch => string.Join(string.Empty, ch.chromosomeArray)).Select(ch => ch.First()).ToList();
            if (newPopulationList.Count < PopulationSize)
            {
                int diff = PopulationSize - newPopulationList.Count;
                for (int i = 0; i < diff; i++)
                    newPopulationList.Add(list[i]);
            }
            else
                newPopulationList.RemoveRange(PopulationSize, newPopulationList.Count - PopulationSize);
            return newPopulationList;
        }
    }
}
