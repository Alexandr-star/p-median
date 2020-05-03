using System;
using System.Windows;

namespace ClusteringViz.Windows
{
    /// <summary>
    /// Interaction logic for AddVertexDataCostDialog.xaml
    /// </summary>
    public partial class AddVertexDataCostDialog : Window
    {
        public double cost => CostUpDown.Value ?? 0;

        public AddVertexDataCostDialog(Window owner)
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
