using GraphX.Common.Models;

namespace p_median_problem.Models
{
    /// <summary>
    /// Класс данных для ребер.
    /// </summary>
    public class DataEdge : EdgeBase<DataVertex>
    {
        public double weigthR { get; set; }
        public double weigthA { get; set; }
        public double weigthM { get; set; }


        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="source">Стартовая вершина.</param>
        /// <param name="target">Конечная вершина.</param>
        /// <param name="weight">Вес ребра.</param>
        public DataEdge(DataVertex source, DataVertex target, double weight = 1, double weigthR = 0, double weigthA = 0, double weigthM = 0)
            : base(source, target, weight)
        {
            this.weigthR = weigthR;
            this.weigthA = weigthA;
            this.weigthM = weigthM;
        }

        /// <summary>
        /// Конструктор без параметров для совместимости с сериализацией.
        /// </summary>
        public DataEdge() : base(null, null, 1)
        {
        }
    }
}
