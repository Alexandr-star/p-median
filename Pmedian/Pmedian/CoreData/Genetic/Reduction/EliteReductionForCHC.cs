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
            if (childList.Count == 0)
                return parentList;
            list.AddRange(parentList);
            list.AddRange(childList);
            list.Sort((first, second) => second.fitness.CompareTo(first.fitness));
            if (list.Count == PopulationSize)
                return list;
            list.RemoveRange(PopulationSize, list.Count - PopulationSize);
            
            return list;
        }
    }
}
