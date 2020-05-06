using System.ComponentModel;

namespace Pmedian.Model.Enums
{
    /// <summary>
    /// Генетические Алгоритмы поиска решения
    /// </summary>
    public enum GeneticAlgotithmMethod
    {
        [Description("Classic GA")]
        ClassicGA,
        [Description("Genitor GA")]
        GenitorGA,
        [Description("CHC GA")]
        CHCGA
    }
}
