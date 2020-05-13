
namespace Pmedian.CoreData.DataStruct
{
    /// <summary>
    /// Класс, реализующий расходы, задаваемый временем скорой помощи и временем фельдшеров.
    /// </summary>
    public class CostEdge
    {
        /// <summary>
        /// Время скорой помощи.
        /// </summary>
        public double timeAmbulance { get; private set; }

        /// <summary>
        /// Время фельдшеров
        /// </summary>
        public double timeMedic { get; private set; }

        public double roadKm { get; private set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="timeAmbulance"></param>
        /// <param name="timeMedic"></param>
        public CostEdge(double weidthRoad, double timeAmbulance, double timeMedic)
        {
            this.roadKm = weidthRoad;
            this.timeAmbulance = timeAmbulance;
            this.timeMedic = timeMedic;
        }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public CostEdge() 
        {
            this.roadKm = 0.0;
            this.timeAmbulance = 0.0;
            this.timeMedic = 0.0;
        }
    }
}
