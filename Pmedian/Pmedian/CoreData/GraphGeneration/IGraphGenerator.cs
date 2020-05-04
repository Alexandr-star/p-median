using Pmedian.Model;

namespace Pmedian.CoreData.GraphGeneration
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
