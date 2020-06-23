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
        /// Заданное количество вершин для генерации графа.
        /// </summary>
        public int VertexCount => VerticesUpDown.Value ?? 0;

        /// <summary>
        /// Заданное количество ребер для генерации графа.
        /// </summary>
        public int MedCount => MedUpDown.Value ?? 0;

        /// <summary>
        /// Средняя степень вершины графа.
        /// </summary>
        public int VillCount => VillUpDown.Value ?? 0;
       
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
                Generator = new SimpleRandom(VertexCount, MedCount, VillCount);
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
        
    }
}

