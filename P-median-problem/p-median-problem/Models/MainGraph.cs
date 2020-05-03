using QuickGraph;

namespace ClusteringViz.Models
{
    /// <summary>
    /// Класс, описывающий граф, наследуемый от BidirectionalGraph и использующий заданные типы данных.
    /// Здесь хранятся данные о вершинах и ребрах, используемые классом GraphArea.
    /// </summary>
    public class MainGraph : BidirectionalGraph<DataVertex, DataEdge> { }
}
