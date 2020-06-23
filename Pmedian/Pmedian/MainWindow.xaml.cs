using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Algorithms.LayoutAlgorithms;
using Microsoft.Win32;
using Pmedian.CoreData;
using Pmedian.CoreData.DataStruct;
using Pmedian.Exceptions;
using Pmedian.FileSeralization;
using Pmedian.Model;
using Pmedian.Model.Enums;
using Pmedian.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;


namespace Pmedian
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        /// <summary>
        /// Текущая операция редактора графов.
        /// </summary>
        private EditorOperationMode operationMode = EditorOperationMode.Select;

        /// <summary>
        /// Объект класса VertexControl, служащий для создания ребер.
        /// </summary>
        private VertexControl sourceVertex;

        /// <summary>
        /// Параметры задачи.
        /// </summary>
        private ProblemData problemData;

        private List<DataEdge> dataEdgeStore;
        private List<EdgeControl> edgeControlStore;

        private MainGraph restoreGraphArea;

        private int countPoint => CountPoint();

        private int CountPoint()
        {
            int count = 0;
            MainGraph graph = graphArea.LogicCore.Graph as MainGraph;
            foreach (var vertex in graph.Vertices)
            {
                if (vertex.Type != VertexType.GroupeVillage)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Включение панели навигации
            ZoomControl.SetViewFinderVisibility(zoomCtrl, Visibility.Visible);
            // Установка масштаба чтобы весь граф был виден
            zoomCtrl.ZoomToFill();
            // Настройка значений класса MainGraphArea
            MainGraphArea_Setup();

            Loaded += MainWindow_Loaded;
        }
        /// <summary>
        /// Метод, вызываемый после загрузки объекта MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Создание сконфигурированного логикой графа
            graphArea.GenerateGraph();

            graphArea.SetEdgesDashStyle(EdgeDashStyle.Solid);
            graphArea.ShowAllEdgesArrows(false);
            graphArea.ShowAllEdgesLabels(false);

            EnableSelectMode();
        }

        /// <summary>
        /// Создание и настройка объекта логики.
        /// </summary>
        private void MainGraphArea_Setup()
        {
            var logicCore = new MainGXLogicCore() { Graph = new MainGraph() };

            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            // Включение асинхронного выполнения методов вроде Area.RelayoutGraph() и Area.GenerateGraph() с пользовательским интерфейсом
            logicCore.AsyncAlgorithmCompute = false;

            graphArea.LogicCore = logicCore;
        }

        /// <summary>
        /// Вызывается, когда была выделена какая-либо вершина.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void graphArea_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                switch (operationMode)
                {
                    case EditorOperationMode.Create:
                        CreateEdgeControl(args.VertexControl);
                        break;
                    case EditorOperationMode.CreateVillage:
                        CreateEdgeControl(args.VertexControl);
                        break;                  
                    case EditorOperationMode.Delete:
                        SafeRemoveVertex(args.VertexControl);
                        break;
                }
            }
        }

        /// <summary>
        /// Вызывается, когда было выделено какое-либо ребро.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void graphArea_EdgeSelected(object sender, EdgeSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed && operationMode == EditorOperationMode.Delete)
                graphArea.RemoveEdge(args.EdgeControl.Edge as DataEdge, true);
        }

        /// <summary>
        /// Вызывается, когда был сделан клик по панели отображения графа.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void zoomCtrl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (operationMode == EditorOperationMode.Create)
                {
                    PutVertexOnGraphArea(VertexType.Unmarket, e);
                }
                else if (operationMode == EditorOperationMode.CreateVillage)
                {
                    PutVertexOnGraphArea(VertexType.GroupeVillage, e);
                }
                else if (operationMode == EditorOperationMode.Select)
                {
                    ClearSelectMode(true);
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                ClearSelectMode();
                EnableSelectMode();
            }
        }

        /// <summary>
        /// Вызывается, когда сделан клик по панели и необходимо поставить вершину на панель
        /// </summary>
        /// <param name="vertexType">Тип вершины</param>
        /// <param name="e">Событие клика мышки</param>
        private void PutVertexOnGraphArea(VertexType vertexType, MouseButtonEventArgs e)
        {
            var pos = zoomCtrl.TranslatePoint(e.GetPosition(zoomCtrl), graphArea);
            pos.Offset(-22.5, -22.5);
            var vc = CreateVertexControl(pos, vertexType);
            if (sourceVertex != null)
                CreateEdgeControl(vc);
        }

        /// <summary>
        /// Вызывается, когда какая-либо из кнопок тулбара была активирована.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolbarButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == buttonCreateMode)
            {
                EnableCreateMode();
            }
            else if (sender == buttonCreateVillageMode)
            {
                EnableCreateVillageMode();
            }
            else if (sender == buttonDeleteMode)
            {
                EnableDeleteMode();
            }
            else
            {
                EnableSelectMode();
            }
        }

        /// <summary>
        /// Вызывается, когда какая-либо из кнопок тулбара была деактивирована.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolbarButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (
                buttonCreateMode.IsChecked == false &&
                buttonDeleteMode.IsChecked == false &&
                buttonCreateVillageMode.IsChecked == false)
            {
                EnableSelectMode();
            }
        }

        /// <summary>
        /// Включает режим выделения редактора графов.
        /// </summary>
        private void EnableSelectMode()
        {
            
            buttonDeleteMode.IsChecked = false;
            zoomCtrl.Cursor = Cursors.Arrow;
            operationMode = EditorOperationMode.Select;
            ClearCreateMode();
            graphArea.SetVerticesDrag(true, true);
            //graphArea.ResetVertexStyle();
        }

        /// <summary>
        /// Включает режим создания редактора графов.
        /// </summary>
        private void EnableCreateMode()
        {
            buttonDeleteMode.IsChecked = false;
            buttonCreateVillageMode.IsChecked = false;
            zoomCtrl.Cursor = Cursors.Hand;
            operationMode = EditorOperationMode.Create;
            ClearSelectMode();
            //graphArea.ResetVertexStyle();
        }

        /// <summary>
        /// Включает режим создания редактора графов для типа вершин деревень.
        /// </summary>
        private void EnableCreateVillageMode()
        {
            buttonCreateMode.IsChecked = false;
            buttonDeleteMode.IsChecked = false;
            zoomCtrl.Cursor = Cursors.Hand;
            operationMode = EditorOperationMode.CreateVillage;
            ClearSelectMode();
            //graphArea.ResetVertexStyle();
        }
             
        /// <summary>
        /// Включает режим удаления редактора графов.
        /// </summary>
        private void EnableDeleteMode()
        {
            buttonCreateMode.IsChecked = false;
            buttonCreateVillageMode.IsChecked = false;
            zoomCtrl.Cursor = Cursors.Hand;
            operationMode = EditorOperationMode.Delete;
            ClearCreateMode();
            ClearSelectMode();
            //graphArea.ResetVertexStyle();
        }

        /// <summary>
        /// Очищает режим выделения.
        /// </summary>
        /// <param name="soft"></param>
        private void ClearSelectMode(bool soft = false)
        {
            graphArea.VertexList.Values
                .Where(DragBehaviour.GetIsTagged)
                .ToList()
                .ForEach(a =>
                {
                    HighlightBehaviour.SetHighlighted(a, false);
                    DragBehaviour.SetIsTagged(a, false);
                });

            if (!soft)
                graphArea.SetVerticesDrag(false);
        }

        /// <summary>
        /// Очищает режим создания.
        /// </summary>
        private void ClearCreateMode()
        {
            if (sourceVertex != null) HighlightBehaviour.SetHighlighted(sourceVertex, false);
            sourceVertex = null;
        }

        /// <summary>
        /// Создает вершину и ее визуальную часть.
        /// </summary>
        /// <param name="position">Позиция вершины.</param>
        /// <returns>объект класса VertexControl.</returns>
        private VertexControl CreateVertexControl(Point position, VertexType vertexType)
        {
            double vertexCost = 0.0;
            if (vertexType == VertexType.Unmarket)
            {
                var dlg = new AddVertexDataCostDialog(this);
                if (dlg.ShowDialog() == false) return null;
                vertexCost = dlg.cost;
            }
                                                            
            var data = new DataVertex(vertexType, vertexCost);
            switch (vertexType)
            {
                case VertexType.GroupeVillage:
                    data.Color = VertexColor.GroupeVillage;
                    break;
                case VertexType.Unmarket:
                    data.Color = VertexColor.Unmarked;
                    break;
                    
            }

          
            var control = new VertexControl(data);

            
            switch (data.Color)
            {
                case VertexColor.GroupeVillage:
                    control.Style = App.Current.Resources["VillageGroupVertex"] as Style;
                    break;
                case VertexColor.Unmarked:
                    control.Style = App.Current.Resources["DefaultVertex"] as Style;
                    break;                
            }

            control.SetPosition(position);
           
            graphArea.AddVertexAndData(data, control, true);
            ClearCreateMode();
            return control;
        }

      
        /// <summary>
        /// Создает ребро и его визуальную часть.
        /// </summary>
        /// <param name="targetVertex">Вершина, инцидентная ребру.</param>
        private void CreateEdgeControl(VertexControl targetVertex)
        {
            if (sourceVertex == null)
            {
                sourceVertex = targetVertex;
                HighlightBehaviour.SetHighlighted(sourceVertex, true);
                return;
            }
            if (sourceVertex == targetVertex) return;
            if (!isUniqueEdge(sourceVertex, targetVertex)) return;

            var dlg = new AddEdgeWigthDialog(this);
            if (dlg.ShowDialog() == false) return;

            double weidthR = dlg.RoadLength;

            var data = new DataEdge((DataVertex)sourceVertex.Vertex, (DataVertex)targetVertex.Vertex, weidthR);
            var control = new EdgeControl(sourceVertex, targetVertex, data);
            
            graphArea.InsertEdgeAndData(data, control, 0, false);
            
            HighlightBehaviour.SetHighlighted(sourceVertex, false);
            sourceVertex = null;
        }

        /// <summary>
        /// Проверяет ребро на уникальность.
        /// </summary>
        /// <param name="sourceVertex">Стартовая вершина.</param>
        /// <param name="targetVertex">Конечная вершина.</param>
        /// <returns>истину, если ребро уникально.</returns>
        private bool isUniqueEdge(VertexControl sourceVertex, VertexControl targetVertex)
        {
            var edgesList = graphArea.EdgesList.Values.ToList();

            foreach (EdgeControl edge in edgesList)
            {
                if (edge.Source == sourceVertex && edge.Target == targetVertex) return false;
                if (edge.Source == targetVertex && edge.Target == sourceVertex) return false;
            }

            return true;
        }

        /// <summary>
        /// Метод, вызываемый после клика на кнопку "Clear Graph".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClearGraph_Click(object sender, RoutedEventArgs e)
        {
            if (graphArea.LogicCore.Graph == null || graphArea.VertexList.Count < 1)
            {
                graphArea.ClearLayout();
                zoomCtrl.ZoomToOriginal();
            }
            else
            {
                var result = MessageBox.Show("Это действие приведет к удалению всего графа. " +
                    "Вы хотите продолжить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    graphArea.ClearLayout();
                    graphArea.LogicCore.Graph.Clear();
                    zoomCtrl.ZoomToOriginal();
                    graphArea.UpdateLayout();
                }
            }
        }

        private void buttonRestoreGraph_Click(object sender, RoutedEventArgs e)
        {
            if (dataEdgeStore.Count != edgeControlStore.Count)
            {
                MessageBox.Show("Что-то пошло не так", "Ошибка");
                return;
            }
            var result = MessageBox.Show("Это действие приведет к удалению всего графа. " +
                    "Вы хотите продолжить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                for (int i = 0; i < dataEdgeStore.Count; i++)
                {
                    graphArea.InsertEdgeAndData(dataEdgeStore[i], edgeControlStore[i], 0, true);
                }
                dataEdgeStore.Clear();
                edgeControlStore.Clear();
            }
            
        }

        /// <summary>
        /// Полностью удаляет вершину и все смежные с ней ребра.
        /// </summary>
        /// <param name="control">Объект VertexControl вершины.</param>
        private void SafeRemoveVertex(VertexControl control)
        {
            graphArea.RemoveVertexAndEdges(control.Vertex as DataVertex);
        }

        /// <summary>
        /// Для взможности динамического создания и удаления объекта GraphArea.
        /// Метод гарантирует, что все занимающие память обхекты будут удалены.
        /// </summary>
        public void Dispose()
        {
            graphArea.Dispose();
        }

        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "File - New".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileNew_Click(object sender, RoutedEventArgs e)
        {
            buttonClearGraph_Click(sender, e);
        }

        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "File - Open".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "XML Files|*.XML" };
            if (dlg.ShowDialog() != true) return;

            try
            {
                graphArea.RebuildFromSerializationData(FileServiceProviderWPF.DeserializeDataFromFile(dlg.FileName));
                graphArea.SetVerticesDrag(true, true);

                EnableSelectMode();
                graphArea.UpdateAllEdges();
                graphArea.UpdateVertexStyle();
                zoomCtrl.ZoomToFill();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки графа.", "Ошибка");
            }
        }

        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "File - Save".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "XML Files|*.XML" };
            if (dlg.ShowDialog() != true) return;

            try
            {
                FileServiceProviderWPF.SerializeDataToFile(dlg.FileName, graphArea.ExtractSerializationData());
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки графа.", "Ошибка");
            }
        }

        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "File - Exit".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "Generate Graph".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void menuGenerateGraph_Click(object sender, RoutedEventArgs e)
        //{
        //    var dlg = new GraphGeneratorDialog(this);
        //    if (dlg.ShowDialog() == false) return;

        //    graphArea.ClearLayout();

        //    // Supplied graph will be automaticaly be assigned to GraphArea::LogicCore.Graph property
        //    graphArea.GenerateGraph(dlg.Generator.Generate());
        //    graphArea.UpdateVertexStyle();
        //    // Fix auto-generated labels (maybe there is a better way, but who cares lol)
        //    graphArea.VertexList.Values.ToList().ForEach(v => v.DetachLabel());

        //    EnableSelectMode();
        //    graphArea.RelayoutGraph();
        //    zoomCtrl.ZoomToFill();
        //}

        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "Redraw Graph".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuRedrawGraph_Click(object sender, RoutedEventArgs e)
        {
            graphArea.RelayoutGraph();
            zoomCtrl.ZoomToFill();
        }


        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "Save as PNG".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveAsPng_Click(object sender, RoutedEventArgs e)
        {
            graphArea.ExportAsImageDialog(ImageType.PNG, true, 96D, 100);
        }

        /// <summary>
        /// Метод, вызываемый после клика на пункт меню "About".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("     Программа Александра Скворцова ИВТ-42БО.\n\n\n" , "О Программе", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void menuSettingGeneticAlgoritm_Click(object sender, RoutedEventArgs e)
        {
            if (graphArea.VertexList.Count < 3)
            {
                MessageBox.Show("Алгоритм не может быть начат. Граф должен содержать хотябы три вершины", "Ошибка");
                return;
            }

            var dlg = new GeneticAlgorithmSetupDialog(this);
            if (dlg.ShowDialog() != true) return;

            EnableSelectMode();

            try
            {

                var result = dlg.GA.GeneticAlgorithm(graphArea.LogicCore.Graph as MainGraph, problemData);
                VisualResult(result);
                new AlgorithmDataWork(this).UpdateData(dlg.GA.GetAlgorithmInfo());
            }
            catch (GeneticAlgorithmException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Что-то пошло не так.", "Ошибка");
                return;
            }
        }

        private void VisualResult(int result)
        {
            if (result == 0)
            {
                MessageBox.Show("Алгоритм не нашел ответ.", "Нет ответа.");
                return;
            }
            else
            {
                graphArea.UpdateVertexStyle();
            }                      
        }

        private void menuMatrixRoad_Click(object sender, RoutedEventArgs e)
        {
            try {                
                if (problemData.RoadCost == 0 || graphArea.VertexList.Count < 3)
                {
                    MessageBox.Show("Не может быть начато. Должен содержать хотя бы три вершины.", "Ошибка");
                    return;
                }
                Cost cost = Cost.GanerateCostArray(graphArea.LogicCore.Graph as MainGraph, problemData);
                var dlg = new MatrixRoadDialog(this, cost, problemData, graphArea.LogicCore.Graph as MainGraph);
                if (dlg.ShowDialog() != true) return;
                EnableSelectMode();
            }
            catch (GeneticAlgorithmException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
                return;
            }

        }

        private void menuSettingProblem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ProblemSetupDialog(this);
            if (dlg.ShowDialog() != true) return;

            EnableSelectMode();

            try
            {
                problemData = dlg.problemData;
                
                if (dlg.P > countPoint)
                {
                    MessageBox.Show("Не может быть начато. Должен содержать хотя бы одну вершину.", "Ошибка");
                    return;
                }
            }
            catch (GeneticAlgorithmException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
                return;
            }

        }
    }
}
