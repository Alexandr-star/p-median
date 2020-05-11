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
        /// <returns>Список отобранных особоей.</returns>
        List<int[]> Selection(List<int[]> population);
    }
}
