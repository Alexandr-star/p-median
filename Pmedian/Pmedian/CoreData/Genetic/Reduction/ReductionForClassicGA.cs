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

        public void Reduction (List<Chromosome> childPopulationList, List<Chromosome> parentPopulationList, List<Chromosome> populationList)
        {                       
            for (int i = 0; i < parentPopulationList.Count; i++)
            {
                for (int j = 0; j < populationList.Count; j++)
                {
                    if (populationList[j].Equals(parentPopulationList[i]))
                    {                        
                        populationList.RemoveAt(j);
                        populationList.Insert(j, childPopulationList[i]); 
                        break;                       
                    }
                }
            } 
            

        }

    }
}
