using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian
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

        public static int[] ShuffleIndexes(int size)
        {
            int[] indexes = new int[size];
            for (int i = 0; i < size; i++)
                indexes[i] = i;

            int randomIndex = 0;
            for (int i = 0; i < size; i++)
            {
                randomIndex = Utility.Rand.Next(size);
                Utility.Swap<int>(ref indexes[i], ref indexes[randomIndex]);
            }

            return indexes;
        }
    }
}
