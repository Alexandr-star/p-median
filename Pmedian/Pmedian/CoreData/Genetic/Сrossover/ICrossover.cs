using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Сrossover
{
    /// <summary>
    /// Интерфейс для алгоритмов кроссовера.
    /// </summary>
    interface ICrossover
    {
        /// <summary>
        /// Кроссовер двух родителей.
        /// </summary>
        /// <param name="firstParant">Первый родитель.</param>
        /// <param name="secondParent">Второй родитель.</param>
        /// <returns></returns>
        List<Chromosome> Crossover(List<Chromosome> parents);

    }
}
