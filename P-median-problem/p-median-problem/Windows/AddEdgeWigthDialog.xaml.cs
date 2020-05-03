using System;
using System.Windows;



namespace p_median_problem.Windows
{
    /// <summary>
    /// Interaction logic for AddEdgeWigthDialog.xaml
    /// </summary>
    public partial class AddEdgeWigthDialog : Window
    {

        public double RoadLength => RoadUpDown.Value ?? 0;

        public double tAmbulator => AmbulatorUpDown.Value ?? 0;

        public double tMedic => MedicUpDown.Value ?? 0;

        public AddEdgeWigthDialog(Window owner)
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
