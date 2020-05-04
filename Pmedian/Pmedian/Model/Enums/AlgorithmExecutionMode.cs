using System.ComponentModel;

namespace Pmedian.Model.Enums
{
    /// <summary>
    /// Режим выполнения алгоритма.
    /// </summary>
    public enum AlgorithmExecutionMode
    {
        [Description("Automatic")]
        Automatic,
        [Description("User Manual")]
        Manual,
        [Description("Result Only")]
        ResultOnly
    }
}
