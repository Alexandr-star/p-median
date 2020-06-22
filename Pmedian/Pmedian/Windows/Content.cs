using Pmedian.CoreData;
using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
using Pmedian.Model.Enums;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pmedian.Windows
{
    public class Content
    {
        private ContentControl contentControl;
                  
        public Content(ContentControl contentControl, Cost cost, ProblemData problemData, MainGraph graph)
        {
            this.contentControl = contentControl;
            PrintMatrix(cost, problemData, graph);
        }
                
        void PrintMatrix(Cost cost, ProblemData problemData, MainGraph graph)
        {
            var grid = new Grid();
            for (int i = 0; i < cost.costEdgeArray[0].Length + 1; i++)
            {                
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());              
            }
            
            for (int i = 0; i < cost.costEdgeArray.Length + 1; i++)
            {
                for (int j = 0; j < cost.costEdgeArray[0].Length + 1; j++)
                {

                    TextBox textBox = new TextBox();
                    textBox.BorderBrush = Brushes.Gray;
                    textBox.BorderThickness = new Thickness(1);
                    textBox.IsReadOnly = true;
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);


                    if (i == 0 && j == 0)
                    {
                        textBox.Background = Brushes.LightGray;
                        textBox.Text = "";
                    }
                    else if (i == 0)
                    {
                        if (cost.costEdgeArray[0][j - 1].roadKm == 0)
                        {
                            continue;
                            //textBox.Background = Brushes.Gray;
                        }
                        else
                        {
                            textBox.Background = Brushes.AliceBlue;
                        }
                        textBox.Text = (j).ToString();
                    }
                    else if (j == 0)
                    {
                        int iVertex = cost.unmarketVertex[i - 1];
                        var vertex = graph.Vertices.ToList().ElementAt(iVertex);
                        if (iVertex + 1 == vertex.ID)
                        {
                            if (vertex.Color == VertexColor.GroupeClinic)
                                textBox.Background = Brushes.Green;
                            else if (vertex.Color == VertexColor.GroupeMedic)
                                textBox.Background = Brushes.Orange;
                            else if (vertex.Color == VertexColor.Unmarked)
                                textBox.Background = Brushes.LightGray;

                        }
                        textBox.Text = (iVertex + 1).ToString() + ": " + cost.costVertexArray[i - 1].ToString();
                    }
                    else if ((i != 0 && j != 0) || j != 0 || i != 0)
                    {
                        if (cost.costEdgeArray[i - 1][j - 1].roadKm == 0)
                            continue;
                        
                        textBox.Background = Brushes.White;
                        textBox.Text = (cost.costEdgeArray[i - 1][j - 1].roadKm * problemData.RoadCost).ToString();
                    }
                    grid.Children.Add(textBox);
                    
                }
            }
            contentControl.Content = grid;
        }
    }
}