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
            //var childIdent = Enumerable.Repeat<int>(1, childList.Count).ToArray();
            //var parantIdent = Enumerable.Repeat<int>(0, parentList.Count).ToArray();
            //var ident = Enumerable.Repeat<int>(0, parentList.Count).ToArray().Concat(Enumerable.Repeat<int>(1, childList.Count).ToArray())                .ToArray();
            
            for (int i = 0; i < parentList.Count; i++)
            {
                if (childList.Count == 0)
                    break;
                for (int j = 0; j < childList.Count; j++)
                {
                    if (parentList[i].fitness == childList[j].fitness)
                    {
                        childList.RemoveAt(j);
                    }
                }
            }
            
            list.AddRange(parentList);
            list.AddRange(childList);
            for (int i = 0; i < list.Count; i++)
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

            list.RemoveRange(parentList.Count, parentList.Count - childList.Count);
            
            return list;
        }
    }
}
