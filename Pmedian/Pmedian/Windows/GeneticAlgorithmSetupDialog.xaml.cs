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
        private GeneticAlgotithmMethod Algorithm => (GeneticAlgotithmMethod)AlgorithmBox.SelectedValue;

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
        private double CrossoverProbability => Math.Round(ProbabilitiCross.Value ?? 0, 3);
        

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
        private double MutationProbability => Math.Round(ProbabilitiMuta.Value ?? 0, 3);

        /// <summary>
        /// Точек в мутации.
        /// </summary>
        private int DotMutation => DotMuta.Value ?? 0;

        /// <summary>
        /// Минимальное хемминговое расстояние.
        /// </summary>
        private int MinHemmingDistance => HemmingDist.Value ?? 0;

        private int CountSelected => SizeSelected.Value ?? 2;

        private int CountTour => SizeTournament.Value ?? 2;

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

            AlgorithmBox.SelectionChanged += AlgorithmBox_SelectionChanged;
            AlgorithmBox_SelectionChanged(this, null);
            CrossoverBox.SelectionChanged += CrossoverBox_SelectionChanged;
            CrossoverBox_SelectionChanged(this, null);
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
                switch (Algorithm)
                {
                    case GeneticAlgotithmMethod.ClassicGA:
                        ErrorMessageZeroIterationSize();
                        GA = new ClassicGA(
                            IterationSize, PopulationSize,
                            CrossoverProbability, MutationProbability,
                            CountSelected, CountTour);
                        break;
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

        private void AlgorithmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Algorithm)
            {
                case GeneticAlgotithmMethod.ClassicGA:
                    PopulatinPanel.Visibility = Visibility.Visible;

                    CrossoverPanel.Visibility = Visibility.Collapsed;
                    ProbabilityCrossPanel.Visibility = Visibility.Visible;
                    HemmingDistansePanel.Visibility = Visibility.Collapsed;
                    
                    MutationPanel.Visibility = Visibility.Collapsed;
                    ProbabilitiMutaPanel.Visibility = Visibility.Visible;

                    SelectionPanel.Visibility = Visibility.Collapsed;
                    
                    break;
                case GeneticAlgotithmMethod.GenitorGA:
                    PopulatinPanel.Visibility = Visibility.Visible;

                    CrossoverPanel.Visibility = Visibility.Visible;
                    ProbabilityCrossPanel.Visibility = Visibility.Collapsed;
                    
                    MutationPanel.Visibility = Visibility.Visible;

                    SizeSelectedPanel.Visibility = Visibility.Collapsed;
                    TournamentPanel.Visibility = Visibility.Collapsed;
                    SelectionNamePanel.Visibility = Visibility.Collapsed;
                    SelectionPanel.Visibility = Visibility.Collapsed;
                    break;
                case GeneticAlgotithmMethod.CHCGA:
                    PopulatinPanel.Visibility = Visibility.Visible;
                    
                    CrossoverPanel.Visibility = Visibility.Collapsed;
                    DotCrossPanel.Visibility = Visibility.Collapsed;
                    ProbabilityCrossPanel.Visibility = Visibility.Collapsed;
                    HemmingDistansePanel.Visibility = Visibility.Visible;

                    MutationPanel.Visibility = Visibility.Visible;
                    ProbabilitiMutaPanel.Visibility = Visibility.Collapsed;
                    
                    SelectionPanel.Visibility = Visibility.Collapsed;
                    SizeSelectedPanel.Visibility = Visibility.Collapsed;
                    TournamentPanel.Visibility = Visibility.Collapsed;
                    SelectionNamePanel.Visibility = Visibility.Collapsed;
                    SelectionPanel.Visibility = Visibility.Collapsed;
                    break;
            }

        }

        private void CrossoverBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CMethod)
            {
                case CrossoverMethod.OneDot:
                    ProbabilityCrossPanel.Visibility = Visibility.Visible;
                    HemmingDistansePanel.Visibility = Visibility.Collapsed;
                    DotCrossPanel.Visibility = Visibility.Collapsed;
                    break;
                case CrossoverMethod.NDot:
                    HemmingDistansePanel.Visibility = Visibility.Collapsed;
                    DotCrossPanel.Visibility = Visibility.Visible;
                    if (Algorithm == GeneticAlgotithmMethod.GenitorGA)
                        ProbabilityCrossPanel.Visibility = Visibility.Collapsed;
                    else
                        ProbabilityCrossPanel.Visibility = Visibility.Visible;
                    break;
                case CrossoverMethod.HUX:
                    HemmingDistansePanel.Visibility = Visibility.Visible;
                    DotCrossPanel.Visibility = Visibility.Collapsed;
                    ProbabilityCrossPanel.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
