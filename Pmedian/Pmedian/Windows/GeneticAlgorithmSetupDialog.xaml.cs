using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Algorithm;
using Pmedian.Model.Enums;
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
    /// Interaction logic for GeneticAlgorithmSetupDialog.xaml
    /// </summary>
    public partial class GeneticAlgorithmSetupDialog : Window
    {
        /// <summary>
        /// Выбранный генетический алгоритм. 
        /// </summary>
        public GeneticAlgotithmMethod Method => (GeneticAlgotithmMethod)AlgorithmBox.SelectedValue;

        /// <summary>
        /// Количество итераций генетического алгоритма.
        /// </summary>
        public int IterationSize => IterSize.Value ?? 0;

        /// <summary>
        /// Выбранный генетический алгоритм.
        /// </summary>
        public IGeneticAlgorithm GA { get; private set; }

        public GeneticAlgorithmSetupDialog(Window owner)
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
                switch (Method)
                {
                    case GeneticAlgotithmMethod.GenitorGA:
                        ErrorMessageZeroIterationSize();
                        GA = new GenitorGA(IterationSize);
                        break;
                    default:
                        ErrorMessageZeroIterationSize();
                        GA = new GenitorGA(IterationSize);
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
        /// Метод, выводящий сообщенеи об ошибке о том, что количество итераци ГА равно 0.
        /// </summary>
        private void ErrorMessageZeroIterationSize()
        {
            if (IterationSize == 0)
            {
                MessageBox.Show("Whoops, something went wrong.", "Iteration size = 0. \n Error");
                return;
            }
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
