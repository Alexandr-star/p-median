using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Сrossover
{    
   public abstract class AbstractCrossover : ICrossover
   {
        /// <summary>
        /// Вероятность кроссовера.
        /// </summary>
        public double Probability { get; }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="probability">Вероятность кроссовера.</param>
        public AbstractCrossover(double probability)
        {
            this.Probability = probability / 100;
        }

        /// <summary>
        /// Кроссовера.
        /// </summary>
        /// <param name="parents">Список родителей.</param>
        /// <returns>Список потомков.</returns>
        public abstract List<int[]> Crossover(List<int[]> parents);
        
        /// <summary>
        /// Кроссовера.
        /// </summary>
        /// <param name="firstParent">Первый родитель.</param>
        /// <param name="secondParent">Второй родитель.</param>
        /// <returns>Потомок.</returns>
        public abstract Chromosome Crossover(Chromosome firstParent, Chromosome secondParent);      
    }   
}
