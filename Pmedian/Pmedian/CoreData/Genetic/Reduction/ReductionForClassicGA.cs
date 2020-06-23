using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Reduction
{
    class ReductionForClassicGA
    {

        public ReductionForClassicGA()
        {

        }

        public void Reduction (List<Chromosome> childPopulationList, int[] parentIndex, List<Chromosome> populationList)
        {                       
            for (int i = 0; i < parentIndex.Length; i++)
            {                                                          
                populationList.RemoveAt(parentIndex[i]);
                populationList.Insert(parentIndex[i], childPopulationList[i]);                                     
            }             
        }

    }
}
