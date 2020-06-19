
namespace Pmedian.CoreData.DataStruct
{
    /// <summary>
    /// Класс, реализующий расходы, задаваемый временем скорой помощи и временем фельдшеров.
    /// </summary>
    public class CostEdge
    {
        /// <summary>
        /// Расстояние между вершинами.
        /// </summary>
        public double roadKm { get; set; }

        // время скорой
        public double timeС { get; set; }

        // время фельдшеров
        public double timeM { get; set; }

        private double _roadKmUtoV;
        public double RoadUtoV
        { get { return _roadKmUtoV; }
          set { _roadKmUtoV = value; } 
        }

        public bool EmptyCost { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="timeAmbulance"></param>
        /// <param name="timeMedic"></param>
        public CostEdge(double weidthRoad)
        {
            this.roadKm = weidthRoad;
            this._roadKmUtoV = weidthRoad;
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
