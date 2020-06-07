using GraphX.Common.Models;
using YAXLib;

namespace Pmedian.Model
{
    /// <summary>
    /// Класс данных для ребер.
    /// </summary>
    public class DataEdge : EdgeBase<DataVertex>
    {
        //[YAXSerializeAs("value_road")]
        //[YAXAttributeFor("WeightEdge")]
        public double weigthR { get; set; }

        //[YAXSerializeAs("value_time_ambalance")]
        //[YAXAttributeFor("WeightEdge")]
        public double weigthA { get; set; }

        //[YAXSerializeAs("value_time_medic")]
        //[YAXAttributeFor("WeightEdge")]
        public double weigthM { get; set; }


        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="source">Стартовая вершина.</param>
        /// <param name="target">Конечная вершина.</param>
        /// <param name="weight">Вес ребра.</param>
        public DataEdge(DataVertex source, DataVertex target, double weight = 1, double weigthR = 1, double weigthA = 0, double weigthM = 0)
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
