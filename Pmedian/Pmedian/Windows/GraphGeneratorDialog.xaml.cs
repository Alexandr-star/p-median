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
        private int vertexVillage => VerticesVillageUpDown.Value ?? 1;
        private int vertexClinic => VerticesClinicUpDown.Value ?? 1;

        private int vertexAmbulator => VerticesAmbulatorUpDown.Value ?? 1;


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
                Generator = new PmedianGraphGenerate(vertexVillage, vertexClinic, vertexAmbulator);
                
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
