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
                for (int j = 0; j < countTurnament; j++)
                {
                    turnamentList.Add(list[Utility.Rand.Next(list.Count)]);
                }
                turnamentList.Sort((first, second) => first.fitness.CompareTo(second.fitness));
                intermidatePopulation.Add(turnamentList.First());
                list.Remove(turnamentList.First());
                turnamentList.Clear();
            }
            return intermidatePopulation;
        }
    }
}
