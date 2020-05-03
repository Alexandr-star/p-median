using p_median_problem.Models;

namespace p_median_problem.Core.GraphGeneration
{
    /// <summary>
    /// Интерфейс для алгоритмов генерации графов.
    /// </summary>
    public interface IGraphGenerator
    {
        /// <summary>
        /// Генерация нового графа по заданному алгоритму.
        /// </summary>
        /// <returns>Новый экземпляр сгенерированного графа.</returns>
        MainGraph Generate();
    }
}
