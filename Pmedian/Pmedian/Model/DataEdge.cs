using GraphX.Common.Models;
using YAXLib;

namespace Pmedian.Model
{
    /// <summary>
    /// Класс данных для ребер.
    /// </summary>
    public class DataEdge : EdgeBase<DataVertex>
    {


        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="source">Стартовая вершина.</param>
        /// <param name="target">Конечная вершина.</param>
        /// <param name="weight">Вес ребра.</param>
        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
            : base(source, target, weight)
        {
        }

        /// <summary>
        /// Конструктор без параметров для совместимости с сериализацией.
        /// </summary>
        public DataEdge() : base(null, null, 1)
        {
        }
              
    }
}
