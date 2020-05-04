using Pmedian.CoreData.GraphGeneration;
using Pmedian.Model.Enums;
using System;
using System.Windows;

namespace Pmedian.Windows
{
    /// <summary>
    /// Interaction logic for GraphGeneratorDialog.xaml
    /// </summary>
    public partial class GraphGeneratorDialog : Window
    {
        /// <summary>
        /// Алгоритм генерации графа.
        /// </summary>
        public GraphGenerationMethod Method => (GraphGenerationMethod)MethodBox.SelectedValue;

        /// <summary>
        /// Заданное количество вершин для генерации графа.
        /// </summary>
        public int VertexCount => VerticesUpDown.Value ?? 0;

        /// <summary>
        /// Заданное количество ребер для генерации графа.
        /// </summary>
        public int EdgesCount => EdgesUpDown.Value ?? 0;

        /// <summary>
        /// Средняя степень вершины графа.
        /// </summary>
        public int MeanDegree => DegreeUpDown.Value ?? 0;

        /// <summary>
        /// Вероятность добавления ребра между двумя случайными вершинами.
        /// </summary>
        public double Probability => ProbabilityUpDown.Value ?? 0;

        /// <summary>
        /// Выбранный алгоритм генерации графа.
        /// </summary>
        public IGraphGenerator Generator { get; private set; }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="owner">Родительское окно.</param>
        public GraphGeneratorDialog(Window owner)
        {
            Owner = owner;
            InitializeComponent();

            MethodBox.SelectionChanged += MethodBox_SelectionChanged;
            MethodBox_SelectionChanged(this, null);
            
        }
        /// <summary>
        /// Метод, вызываемый после клика на кнопку "OK".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Method)
                {
                    case GraphGenerationMethod.Random:
                        Generator = new SimpleRandom(VertexCount);
                        break;
                    case GraphGenerationMethod.Precise:
                        Generator = new SimplePrecise(VertexCount, EdgesCount);
                        break;
                    case GraphGenerationMethod.WattsStrogatz:
                        Generator = new WattsStrogatz(VertexCount, MeanDegree, Probability);
                        break;
                    case GraphGenerationMethod.ErdosRenyi:
                        Generator = new ErdosRenyi(VertexCount, Probability);
                        break;
                    default:
                        Generator = new SimpleRandom(VertexCount);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Метод, вызываемый после клика на кнопку "Cancel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод, вызываемый при смене текущего алгоритма генерации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MethodBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (Method)
            {
                case GraphGenerationMethod.Random:
                    VerticesPanel.Visibility = Visibility.Visible;
                    EdgesPanel.Visibility = Visibility.Collapsed;
                    DegreePanel.Visibility = Visibility.Collapsed;
                    ProbabilityPanel.Visibility = Visibility.Collapsed;
                    break;
                case GraphGenerationMethod.Precise:
                    VerticesPanel.Visibility = Visibility.Visible;
                    EdgesPanel.Visibility = Visibility.Visible;
                    DegreePanel.Visibility = Visibility.Collapsed;
                    ProbabilityPanel.Visibility = Visibility.Collapsed;
                    break;
                case GraphGenerationMethod.WattsStrogatz:
                    VerticesPanel.Visibility = Visibility.Visible;
                    EdgesPanel.Visibility = Visibility.Collapsed;
                    DegreePanel.Visibility = Visibility.Visible;
                    ProbabilityPanel.Visibility = Visibility.Visible;
                    break;
                case GraphGenerationMethod.ErdosRenyi:
                    VerticesPanel.Visibility = Visibility.Visible;
                    EdgesPanel.Visibility = Visibility.Collapsed;
                    DegreePanel.Visibility = Visibility.Collapsed;
                    ProbabilityPanel.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
