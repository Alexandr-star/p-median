using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Mutation
{
    class TranslocationMutation : AbstractMutation
    {
        public TranslocationMutation(double probability) : base(probability) { }
        public override void Mutation(int[] chromosome)
        {
            throw new NotImplementedException();
        }
    }
}
