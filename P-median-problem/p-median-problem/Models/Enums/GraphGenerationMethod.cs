using System.ComponentModel;

namespace p_median_problem.Models.Enums
{
    /// <summary>
    /// Алгоритм генерации графа.
    /// </summary>
    public enum GraphGenerationMethod
    {
        [Description("Simple Random")]
        Random,
        [Description("Simple Precise")]
        Precise,
        [Description("Watts-Strogatz")]
        WattsStrogatz,
        [Description("Erdos-Renyi")]
        ErdosRenyi
    }
}
