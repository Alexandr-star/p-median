using System.ComponentModel;

namespace ClusteringViz.Models.Enums
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
