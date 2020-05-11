using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Mutation
{
    public abstract class AbstractMutation : IMutation
    {
        // <summary>
        /// Вероятность  мутации.
        /// </summary>
        public double Probability { get; }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="probability">Вероятность мутации.</param>
        public AbstractMutation(double probability)
        {
            this.Probability = probability;
        }

        /// <summary>
        /// Мутация хромосомы.
        /// </summary>
        /// <param name="chromosome">Хромосома.</param>
        public abstract void Mutation(int[] chromosome);
    }
}
