using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.Exceptions
{
    class GeneticAlgorithmException : System.Exception
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public GeneticAlgorithmException() : base() { }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку.</param>
        public GeneticAlgorithmException(string message) : base(message) { }

    }
}
