using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Mutation
{
    class SwapMutation : AbstractMutation
    {
        public SwapMutation(double probability) : base(probability) { }

        public override void Mutation(int[] chromosome)
        {
            throw new NotImplementedException();
        }
    }
}
