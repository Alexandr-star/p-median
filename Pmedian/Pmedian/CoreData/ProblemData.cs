
namespace Pmedian.CoreData
{
    /// <summary>
    /// Структура, описывающая параметры задачи.
    /// </summary>
    public struct ProblemData
    {
        /// <summary>
        /// Минимальное количество медиан у вершины, т.е. у деревни.
        /// </summary>
        public int P { get; }

        /// <summary>
        /// Время приезда фельдшеров, ограниченное числом.
        /// </summary>
        public double MedicTime { get; }
        public double MidSpeedMedic { get; }

        /// <summary>
        /// Время приезда скорой помощи, ограниченное числом.
        /// </summary>
        public double AmbulanceTime { get; }
        public double MidSpeedAmbulance { get; }

        /// <summary>
        /// Затраты на 1 км пути.
        /// </summary>
        public double RoadCost { get; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="p">Минимальное количество медиан.</param>
        /// <param name="timeMedic">Время приезда фельдшеров, ограниченное числом.</param>
        /// <param name="timeAmbulance">Время приезда скорой помощи, ограниченное числом.</param>
        /// <param name="roadCost">Затраты на 1 км пути.</param>
        public ProblemData(int p, double midSpeedMedic, double medicTime, double midSpeedAmbulance, double ambilanceTime, double roadCost)
        {
            P = p;
            MidSpeedMedic = midSpeedMedic;
            MidSpeedAmbulance = midSpeedAmbulance;
            RoadCost = roadCost;
            MedicTime = medicTime;
            AmbulanceTime = ambilanceTime;
        }
    }
}
