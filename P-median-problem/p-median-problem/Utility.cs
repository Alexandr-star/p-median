using System;

namespace p_median_problem
{
    /// <summary>
    /// Вспомогательные иструмены для разработки.
    /// </summary>
    static class Utility
    {
        /// <summary>
        /// Глобальный экземпляр генератора псевдослучайных чисел.
        /// </summary>
        public static readonly Random Rand = new Random(Guid.NewGuid().GetHashCode());
        
        /// <summary>
        /// Выполняет обмен значений двух элементов. 
        /// </summary>
        /// <typeparam name="T">Тип обмениваемых значений.</typeparam>
        /// <param name="lhs">Первый элемент.</param>
        /// <param name="rhs">Второй элемент.</param>
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
