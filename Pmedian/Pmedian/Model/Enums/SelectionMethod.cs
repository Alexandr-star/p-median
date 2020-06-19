using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.Model.Enums
{
    /// <summary>
    /// Ваианты генетического оператора селекции.
    /// </summary>
    public enum SelectionMethod
    {
        [Description("Tournament")]
        Tournament,
        [Description("Proportion")]
        Proportion
    }
}
