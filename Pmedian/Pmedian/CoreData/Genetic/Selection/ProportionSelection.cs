using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Selection
{
    class ProportionSelection : AbstractSelection
    {

        public ProportionSelection(int countSelected) : base(countSelected)
        {

        }
        public override List<Chromosome> Selection(Population population)
        {
           
            List<Chromosome> midPop = new List<Chromosome>();
            //double sumfit = SumAllFitness(population);
            double sumfit = 0.0;
            foreach (var ch in population.populationList)
            {
                sumfit += ch.fitness;
            }

            double previos_prob = 0.0;
            double[] previos = new double[population.SizePopulation];
            double[] courent = new double[population.SizePopulation];
            
            for (int i = 0; i < population.SizePopulation; i++)
            {
                courent[i] = previos_prob + (population.populationList[i].fitness) / sumfit;
                previos[i] = previos_prob;
                previos_prob = courent[i];
            }

            int index = 0;
            while (midPop.Count < countSelected)
            {
                double p = Utility.Rand.NextDouble();
                for (int j = 0; j < population.SizePopulation; j++)
                {
                    if (previos[j] <= p && p <= courent[j])
                    {
                        midPop.Add(population.populationList[j]);
                        indexSelectChrom[index] = j;
                        index++;
                        break;
                    }
                }
            }
            return midPop;   
        }

        /*private double SumAllFitness(List<Chromosome> population)
        {
            
        }*/
    }
}
