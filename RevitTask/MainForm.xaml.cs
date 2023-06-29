using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTask.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace RevitTask
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        Autodesk.Revit.ApplicationServices.Application app;
        internal UIApplication uiapp;
        internal UIDocument uidoc;
        internal Document doc;
        //public static readonly DependencyProperty TaskListProperty =
        //    DependencyProperty.Register(
        //        "YourTaskList", 
        //        typeof(List<Model.Task>), 
        //        typeof(Model.Task), 
        //        new PropertyMetadata(null));
        //public List<Model.Task> TaskList
        //{
        //    get { return (List<Model.Task>)GetValue(TaskListProperty); }
        //    set { SetValue(TaskListProperty, value); }
        //}
        public ObservableCollection<Model.Task> TaskList1 { get; set; }
        public ObservableCollection<Model.Task> TaskList2 { get; set; }
        public ICommand ToggleDetailsCommand { get; }
        public MainForm()
        {

            InitializeComponent();
            this.Initialized += MainForm_Initialized;
            ToggleDetailsCommand = new Controller.ToggleDetailsCommand(ToggleDetailsVisibility);
            TaskList1 = new ObservableCollection<Model.Task>
            {
                new Model.Task
                {
                    Id = 1,
                    ChapterId =1,
                    HasAttachments = true,
                    TaskTime = new DateTime()
                },
                new Model.Task
                {
                    Id = 2,
                    ChapterId =2,
                    HasAttachments = true,
                    TaskTime = new DateTime()



                }
            };
            TaskList2 = new ObservableCollection<Model.Task>
            {
                new Model.Task
                {
                    Id = 3,
                    ChapterId =3,
                    HasAttachments = false,
                    TaskTime = new DateTime()
                },
                new Model.Task
                {
                    Id = 4,
                    ChapterId =4,
                    HasAttachments = false,
                    TaskTime = new DateTime()



                }
            };
            
            DataContext = this;
        }
        internal void SetRevitLinks(UIApplication uIApplication)
        {
            uiapp = uIApplication;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;
        }

        private void MainForm_Initialized(object sender, EventArgs e)
        {
            //using (Model1 db = new Model1())
            //{
            //    var vv = db.Chapter.Select(ch => ch.ChapterName).ToList();
            //}
        }

        private void StackPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Model.Task)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }
        private static T FindVisualParent<T>(DependencyObject child) where T : class
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }
        private T FindVisualChildByName<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            if (parent == null)
                return null;

            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T element && element.Name == name)
                    return element;

                var result = FindVisualChildByName<T>(child, name);
                if (result != null)
                    return result;
            }

            return null;
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Model.Task)))
            {
                Model.Task taskCard = e.Data.GetData(typeof(Model.Task)) as Model.Task;
                FrameworkElement frameworkElement = new FrameworkElement();
                frameworkElement.DataContext = taskCard;

                StackPanel destinationStackPanel = sender as StackPanel;
                StackPanel sourceStackPanel = FindVisualParent<StackPanel>(frameworkElement);

                if (destinationStackPanel != null && sourceStackPanel != null && destinationStackPanel != sourceStackPanel)
                {
                    sourceStackPanel.Children.Remove(frameworkElement);
                    destinationStackPanel.Children.Add(frameworkElement);
                }
            }
        }

        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TextBlock textBlock = sender as TextBlock;
                Model.Task taskCard = textBlock.DataContext as Model.Task;

                if (taskCard != null)
                {
                    DataObject dataObject = new DataObject(typeof(Model.Task), taskCard);
                    DragDrop.DoDragDrop(textBlock, dataObject, DragDropEffects.Move);
                }
            }
        }

        private void DoingStackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Model.Task)))
            {
                Model.Task taskCard = e.Data.GetData(typeof(Model.Task)) as Model.Task;
                StackPanel destinationStackPanel = sender as StackPanel;
                FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;

                if (destinationStackPanel != null && frameworkElement != null)
                {
                    if (destinationStackPanel.Name == "DoneStackPanel")
                    {
                        // If the destination is "Done", block the drop
                        return;
                    }

                    // Move the task card from the source stack panel to the destination stack panel
                    StackPanel sourceStackPanel = FindVisualParent<StackPanel>(frameworkElement);
                    //sourceStackPanel.Children.Remove(frameworkElement);
                    TaskList1.Remove(taskCard);
                    TaskList2.Add(taskCard);
                    //destinationStackPanel.Children.Add(frameworkElement);
                    sourceStackPanel.InvalidateVisual();
                    destinationStackPanel.InvalidateVisual();

                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            TaskList1.Add(
                new Model.Task
                {
                    Id = 3,
                    ChapterId = 3,
                    HasAttachments = true,
                    TaskTime = new DateTime()
                });
            
            //ScrollViewer scrollViewer= FindVisualParent<ScrollViewer>(frameworkElement);
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Border border= sender as Border;
                Model.Task taskCard = border.DataContext as Model.Task;

                if (taskCard != null)
                {
                    DataObject dataObject = new DataObject(typeof(Model.Task), taskCard);
                    DragDrop.DoDragDrop(border, dataObject, DragDropEffects.Move);
                }
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            System.Windows.Controls.Grid par = frameworkElement.Parent as System.Windows.Controls.Grid;
            if (par != null)
            {
                Border border = par.Children
                    .OfType<Border>()
                    .FirstOrDefault(el => el.Name == "DetailsPanel");
                if (border != null)
                {
                    System.Windows.Data.Binding binding = BindingOperations.GetBinding(border, Border.VisibilityProperty);
                    var vis1 = border.Visibility;
                    if (vis1 == System.Windows.Visibility.Visible) border.Visibility = System.Windows.Visibility.Collapsed;

                    vis1 = border.Visibility;

                    

                }
            }

        }
        private void ToggleDetailsVisibility()
        {
            // Код для управления видимостью панели деталей
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            var menuItem = (MenuItem)sender;
            DependencyObject parent = VisualTreeHelper.GetParent(menuItem);
            Border border = FindVisualParent<Border>(menuItem);
            
            if (null != border)
            {
                StackPanel sourceStackPanel = parent as StackPanel;
                if (sourceStackPanel != null)
                {
                    Model.Task taskCard = border.DataContext as Model.Task;
                    TaskList1.Remove(taskCard);
                    sourceStackPanel.InvalidateVisual();

                }


            }

        }
    }
   
}
