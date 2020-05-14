
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

        /// <summary>
        /// Расстояние между вершинами.
        /// </summary>
        public double roadKm { get; private set; }

        public bool EmptyCost { get; private set; }

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
            this.EmptyCost = false;
        }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public CostEdge() 
        {
            this.EmptyCost = true;
        }
    }
}
