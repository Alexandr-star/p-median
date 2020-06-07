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

        public void Reduction (List<Chromosome> childPopulationList, List<Chromosome> parentPopulationList, List<Chromosome> populationList, List<int> ind)
        {
            List<Chromosome> list = new List<Chromosome>();            
            for (int i = 0; i < parentPopulationList.Count; i++)
            {
                if (!ind.Contains(i))
                    list.Add(parentPopulationList[i]);
                
            }
            
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < populationList.Count; j++)
                {
                    if (populationList[j].Equals(list[i]))
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
