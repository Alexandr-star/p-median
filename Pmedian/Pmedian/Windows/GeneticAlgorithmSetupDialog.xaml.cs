using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Algorithm;
using Pmedian.CoreData.Genetic.Сrossover;
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
        private GeneticAlgotithmMethod AMethod => (GeneticAlgotithmMethod)AlgorithmBox.SelectedValue;

        /// <summary>
        /// Количество итераций генетического алгоритма.
        /// </summary>
        private int IterationSize => IterSize.Value ?? 0;

        /// <summary>
        /// Размер популяции.
        /// </summary>
        private int PopulationSize => PopSize.Value ?? 0;

        /// <summary>
        /// Выбранный кроссовер.
        /// </summary>
        private CrossoverMethod CMethod => (CrossoverMethod)CrossoverBox.SelectedValue;

        /// <summary>
        /// Вероятность кроссовера.
        /// </summary>
        private double CrossoverProbability => CrossoverSlider.Value;

        /// <summary>
        /// Точек в кроссовере.
        /// </summary>
        private int DotCrossover => DotCross.Value ?? 0;

        /// <summary>
        /// Выбранная мутация.
        /// </summary>
        private MutationMethod MMethod => (MutationMethod)MutationBox.SelectedValue;

        /// <summary>
        /// Вероятность мутации.
        /// </summary>
        private double MutationProbability => MutationSlider.Value;

        /// <summary>
        /// Точек в мутации.
        /// </summary>
        private int DotMutation => DotMuta.Value ?? 0;

        /// <summary>
        /// Минимальное хемминговое расстояние.
        /// </summary>
        private int MinHemmingDistance => HemmingDist.Value ?? 0;

        /// <summary>
        /// Выбранный генетический алгоритм.
        /// </summary>
        public IGeneticAlgorithm GA { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="owner"></param>
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
                switch (AMethod)
                {
                    case GeneticAlgotithmMethod.GenitorGA:
                        ErrorMessageZeroIterationSize();
                        GA = new GenitorGA(
                            IterationSize, PopulationSize,
                            CMethod, CrossoverProbability, DotCrossover,
                            MMethod, MutationProbability, DotMutation,
                            MinHemmingDistance);
                        break;
                    case GeneticAlgotithmMethod.CHCGA:
                        ErrorMessageZeroIterationSize();
                        GA = new CHCGA(
                            IterationSize, PopulationSize,
                            CrossoverProbability,
                            MMethod, DotMutation,
                            MinHemmingDistance);
                        break;
                    default:
                        ErrorMessageZeroIterationSize();
                        GA = new GenitorGA(
                            IterationSize, PopulationSize,
                            CMethod, CrossoverProbability, DotCrossover,
                            MMethod, MutationProbability, DotMutation,
                            MinHemmingDistance);
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
