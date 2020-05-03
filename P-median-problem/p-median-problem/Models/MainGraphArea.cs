using ClusteringViz.Models.Enums;
using GraphX.Controls;
using QuickGraph;
using System.Linq;
using System.Windows;

namespace ClusteringViz.Models
{
    /// <summary>
    /// Класс, описывающий панель отображения графа с использованием заданных типов данных.
    /// </summary>
    public class MainGraphArea : GraphArea<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    {
        /// <summary>
        /// Обновляет отображаемые цвета вершин.
        /// </summary>
        public void UpdateVertexStyle()
        {
            VertexList.ToList().ForEach(a =>
            {
                switch (a.Key.Color)
                {
                    case VertexColor.Selected:
                        a.Value.Style = App.Current.Resources["SelectedVertex"] as Style;
                        break;
                    case VertexColor.FirstGroup:
                        a.Value.Style = App.Current.Resources["FirstGroupVertex"] as Style;
                        break;
                    case VertexColor.SecondGroup:
                        a.Value.Style = App.Current.Resources["SecondGroupVertex"] as Style;
                        break;
                    default:
                        a.Value.Style = App.Current.Resources["DefaultVertex"] as Style;
                        break;
                }
            });
        }
        
        /// <summary>
        /// Устанавливает цвета всех вершин по умолчанию.
        /// </summary>
        public void ResetVertexStyle()
        {
            VertexList.Keys.ToList().ForEach(a =>
            {
                a.Color = VertexColor.Unmarked;
            });

            UpdateVertexStyle();
        }
    }
}
