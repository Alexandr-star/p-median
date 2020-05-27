using Pmedian.CoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pmedian.Windows
{
    /// <summary>
    /// Interaction logic for ProblemSetupDialog.xaml
    /// </summary>
    public partial class ProblemSetupDialog : Window
    {
        /// <summary>
        /// Затраты на 1 км пути.
        /// </summary>
        private double roadCost => Math.Round(OnRoadCostUpDown.Value ?? 0, 3);

        /// <summary>
        /// Количество пунктов у деревни.
        /// </summary>
        private int _p => PmedianCostUpDown.Value ?? 0;

        public int P { get => _p; }

        /// <summary>
        /// Время фельдшеров.
        /// </summary>
        private double timeMedicCost => Math.Round(TimeMedicCostUpDown.Value ?? 0, 3);

        /// <summary>
        /// Время скорой помощи.
        /// </summary>
        private double timeAmbulaceCost => Math.Round(TimeAmbulanceCostUpDown.Value ?? 0, 3);

        public ProblemData problemData { get; private set; }

        public ProblemSetupDialog(Window owner)
        {
            Owner = owner;
            InitializeComponent();
        }
        /// <summary>
        /// Метод, вызываемый после клика на кнопку "OK".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                problemData = new ProblemData(P, timeMedicCost, timeAmbulaceCost, roadCost);
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
