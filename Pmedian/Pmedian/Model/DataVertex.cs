using GraphX.Common.Models;
using Pmedian.Model.Enums;
using YAXLib;

namespace Pmedian.Model
{
    /// <summary>
    /// Класс данных для вершин.
    /// </summary>
    public class DataVertex : VertexBase
    {
        /// <summary>
        /// Цвет вершины.
        /// </summary>
        public VertexColor Color { get; set; }

        /// <summary>
        /// Тип вершины.
        /// </summary>
        public VertexType Type { get; set; }

        private bool _isVisited = false;
        public bool IsVisited
        {
            get { return _isVisited; }
            set
            {
                _isVisited = value;
            }
        }

        private double _pointValue = double.MaxValue;
        public double PointValue
        {
            get { return _pointValue; }
            set { _pointValue = value; }
        }

        public DataVertex PredPoint { get; set; }

        public double vertexCost { get; set; }

        /// <summary>
        /// Конструктор с возможностью задания цвета вершины.
        /// </summary>
        /// <param name="color">Цвет вершины.</param>
        public DataVertex(VertexColor color)
        {
            Color = color;
        }

        /// <summary>
        /// Конструктор с возможностью задания типа вершины.
        /// </summary>
        /// <param name="type">Тип вершины.</param>
        public DataVertex(VertexType type, double cost)
        {
            this.vertexCost = cost;
            Type = type;
        }

        public DataVertex(VertexType type)
        {
            Type = type;
        }

        /// <summary>
        /// Конструктор с возможностью задания цвета и типа вершины.
        /// </summary>
        /// <param name="color">Цвет вершины.</param>
        /// <param name="type">Тип вершины.</param>
        public DataVertex(VertexColor color, VertexType type)
        {
            Color = color;
            Type = type;
        }
        
        public DataVertex(VertexColor color, VertexType type, double cost)
        {
            Color = color;
            Type = type;
            this.vertexCost = cost;
        }

        /// <summary>
        /// Конструктор без параметров для совместимости с сериализацией.
        /// </summary>
        public DataVertex() : this(VertexColor.GroupeVillage, VertexType.GroupeVillage)
        {
        }
    }
}
