using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Mutation
{
    /// <summary>
    /// Интерфейс для оператора мутации.
    /// </summary>
    interface IMutation
    {
        /// <summary>
        /// Мутация особи.
        /// </summary>
        /// <param name="chromosome">Особь.</param>
        void Mutation(Chromosome chromosome); 
    }
}
