using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Selection
{
    class TournamentSelection : AbstractSelection
    {
        private int countTurnament;

        public TournamentSelection(int countSelected, int countTurnament) : base(countSelected)
        {
            this.countTurnament = countTurnament;
        }

        public override List<Chromosome> Selection(Population population)
        {
            List<Chromosome> intermidatePopulation = new List<Chromosome>(countSelected);
            List<Chromosome> list = new List<Chromosome>();
            list.AddRange(population.populationList);
            List<Chromosome> turnamentList = new List<Chromosome>(countTurnament);

            for (int i = 0; i < countSelected; i++)
            {                
                int tempi = 0;
                for (int j = 0; j < countTurnament; j++)
                {
                    int index = Utility.Rand.Next(list.Count);                                 
                    while (tempi == index)
                    {
                        index = Utility.Rand.Next(list.Count);
                    }
                    turnamentList.Add(list[index]);
                    tempi = index;
                }
                turnamentList.Sort((first, second) => first.fitness.CompareTo(second.fitness));
                intermidatePopulation.Add(turnamentList.First());
                indexSelectChrom[i] = population.populationList.IndexOf(turnamentList.First());
                turnamentList.Clear();
            }
            return intermidatePopulation;
        }
    }
}
