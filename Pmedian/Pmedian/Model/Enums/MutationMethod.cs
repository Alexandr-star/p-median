using System.ComponentModel;

namespace Pmedian.Model.Enums
{
    /// <summary>
    /// Варианты генетического оператора мутации.
    /// </summary>
    public enum MutationMethod
    {
        [Description("Replace Mutation")]
        ReplaceMutation,
        [Description("Inversion Mutation")]
        InversionMutation,
        [Description("Swap Mutation")]
        SwapMutation,
        [Description("Translocation Mutation")]
        TranslocationMutation
    }
}
