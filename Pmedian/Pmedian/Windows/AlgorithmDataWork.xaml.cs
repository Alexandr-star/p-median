using Pmedian.CoreData.Genetic.Algorithm;
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
using System.Windows.Shapes;

namespace Pmedian.Windows
{
    /// <summary>
    /// Interaction logic for AlgorithmDataWork.xaml
    /// </summary>
    public partial class AlgorithmDataWork : Window
    {
        private AlgorithmInfo algorithmInfo;

        public AlgorithmDataWork(MainWindow owner)
        {
            Owner = owner;
            InitializeComponent();

            Loaded += AlgorithmDataWork_Loaded;
            Closed += AlgorithmDataWork_Closed;

            Owner.LocationChanged += UpdatePosition;
            Owner.StateChanged += UpdatePosition;
            Owner.SizeChanged += UpdatePosition;
        }

        /// <summary>
        /// Метод, вызываемый после закрытия окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmDataWork_Closed(object sender, EventArgs e)
        {
            (Owner as MainWindow).MainMenu.IsEnabled = true;
            (Owner as MainWindow).MainSidebar.IsEnabled = true;
        }

        /// <summary>
        /// Метод, вызываемый после загрузки окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmDataWork_Loaded(object sender, RoutedEventArgs e)
        {
            (Owner as MainWindow).MainMenu.IsEnabled = false;
            (Owner as MainWindow).MainSidebar.IsEnabled = false;
        }

        /// <summary>
        /// Обновление позиции окна относительно элементов родителя.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdatePosition(object sender, EventArgs e)
        {
            var sidebarTopLeft = (Owner as MainWindow).MainSidebar.PointToScreen(new Point(0, 0));

            this.Left = sidebarTopLeft.X - this.Width - 8;
            this.Top = sidebarTopLeft.Y;
        }

        /// <summary>
        /// Обновление визуализации для текущего этапа выполнения алгоритма.
        /// </summary>
        public void UpdateData(AlgorithmInfo algorithmInfo)
        {
            Show(); UpdatePosition(this, null);
            labelSteps.Content = String.Format("STEP: {0}", algorithmInfo.Steps);
            labelTime.Content = String.Format("TIME: {0}", algorithmInfo.Time);
            labelStepGainCounter.Content = String.Format("Best F(x): {0}", algorithmInfo.BestFx);

        }
        

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }
    }
}
