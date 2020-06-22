using Pmedian.CoreData;
using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
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
    /// Interaction logic for MatrixRoadDialog.xaml
    /// </summary>
    public partial class MatrixRoadDialog : Window
    {
        public MatrixRoadDialog(Window owner, Cost cost, ProblemData problemData, MainGraph graph)
        {
            Owner = owner;
            InitializeComponent();

            
            DataContext = new Content(Matrix, cost, problemData, graph);
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {            

            DialogResult = true;
            Close();
        }
    }
}
