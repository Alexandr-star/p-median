using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pmedian.CoreData.Genetic.Mutation
{
    class TranslocationMutation : AbstractMutation
    {
        public TranslocationMutation(double probability, int point) : base(probability, point) { }
        

        public override void Mutation(Chromosome chromosome)
        {
            if (Utility.Rand.NextDouble() < Probability)
            {
                int startPoint = 0;
                int locatePoint = 0;
                do {
                     startPoint = Utility.Rand.Next(14 - Point);
                     locatePoint = Utility.Rand.Next(14 - Point);
                } while (startPoint == locatePoint);

                List<int> list = chromosome.chromosomeArray.ToList<int>();
                List<int> listRange = list.GetRange(startPoint, Point);
                list.RemoveRange(startPoint, Point);
                list.InsertRange(locatePoint, listRange);
                chromosome.chromosomeArray = list.ToArray();

            }
        }

        public override void Mutation(List<Chromosome> childChromodome)
        {
            throw new NotImplementedException();
        }
    }
}
