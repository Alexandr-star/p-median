﻿using Pmedian.CoreData.Genetic.Algorithm;
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
        public IGeneticAlgorithm Algorithm { get; private set; }

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
                Algorithm = new Algor();
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