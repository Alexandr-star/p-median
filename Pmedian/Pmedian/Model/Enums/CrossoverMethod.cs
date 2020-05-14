using System.ComponentModel;

namespace Pmedian.Model.Enums
{
    /// <summary>
    /// Варианты генетического оператора кроссовер.
    /// </summary>
    public enum CrossoverMethod
    {
        [Description("One dot Crossover")]
        OneDot,
        [Description("N dot Crossover")]
        NDot
    }
}
