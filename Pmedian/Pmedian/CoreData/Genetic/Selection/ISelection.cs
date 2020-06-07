using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Selection
{
    /// <summary>
    /// Интерфейс для селекции особей.
    /// </summary>
    interface ISelection
    {
        /// <summary>
        /// Селекция особей.
        /// </summary>
        /// <param name="population">Популяция, т.е. список особей.</param>
        /// <param name="N">Количество отбираемых особей</param>
        /// <returns>Список отобранных особоей.</returns>
        List<Chromosome> Selection(List<Chromosome> population);
    }
}
