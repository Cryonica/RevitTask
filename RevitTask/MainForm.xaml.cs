using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using RevitTask.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using static UIFramework.SemanticTree;
using IFCBIM = BIM.IFC.Export.UI;

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
        public ObservableCollection<Model.ServerFilesModel> ServerFiles1 { get; set; }
        public ICommand ToggleDetailsCommand { get; }
        public MainForm()
        {

            InitializeComponent();
            this.Initialized += MainForm_Initialized;
            ToggleDetailsCommand = new Controller.ToggleDetailsCommand(ToggleDetailsVisibility);
            TaskList1 = new ObservableCollection<Model.Task>()
            {
                new Model.Task
                {
                    Id = 1,
                    ChapterId =1,
                   TaskTime = new DateTime()
                },
                new Model.Task
                {
                    Id = 2,
                    ChapterId =2,
                    
                    TaskTime = new DateTime()



                }
            };
            TaskList2 = new ObservableCollection<Model.Task>
            {
                new Model.Task
                {
                    Id = 3,
                    ChapterId =3,
                    TaskTime = new DateTime()
                },
                new Model.Task
                {
                    Id = 4,
                    ChapterId =4,
                    TaskTime = new DateTime()



                }
            };
            CreateTreeFiles();


             //ServerFiles1 = ServerFilesModel.CreateFiles();

             //ServerFilesModel root = ServerFiles1[0];

             //base.CommandBindings.Add(new CommandBinding(
             //       ApplicationCommands.Undo, (sender, e) => // Execute
             //       {
             //           e.Handled = true;
             //           root.IsChecked = false;
             //           tree.Focus();
             //       }, (sender, e) => // CanExecute
             //       {
             //           e.Handled = true;
             //           e.CanExecute = (root.IsChecked != false);
             //       }));
             //tree.Focus();



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
        private static T FindParentByName<T>(DependencyObject child, string name) where T : FrameworkElement
        {
            if (child == null) return null;
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            while (parentObject != null)
            {
                if (parentObject is T parent && parent.Name == name)
                    return parent;
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }
            return null;

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
                    if (sourceStackPanel == destinationStackPanel) return;
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
                if (destinationStackPanel.Name == "DoingStackPanel")
                {
                    if (TaskList2.Contains(taskCard)) return;
                }

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

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            using (Model1 db = new Model1())
            {

                string filePath = @"C:\Users\user\source\repos\RevitTask\RevitTask\Images\checklist.png";
                byte[] fileBytes;
                byte[] hashBytes;

                // Чтение содержимого файла и преобразование его в массив байтов
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        hashBytes = sha256.ComputeHash(fileStream);
                    }

                    fileBytes = File.ReadAllBytes(filePath);
                }
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                Model.TaskFiles taskFiles = new TaskFiles();
                taskFiles.FileName = "checklist.png";
                taskFiles.FileContent = fileBytes;
                taskFiles.SHA256 = hashString;

                Model.Task task = new Model.Task();
                task.FilesPresent = true;
                task.ChapterId = 1;
                task.TaskTime = DateTime.Now;
                task.Comment = "dfdf";
                task.TaskFiles.Add(taskFiles);

                db.Task.Add(task);
                db.SaveChanges();


                var file = db.TaskFiles
                    .Where(ee => ee.SHA256 == hashString)
                    .Select(x => new { x.FileName, x.FileContent }).ToArray();

                var convertedResult = file.Select(
                    x => new Model.TaskFiles
                    {
                        FileName = x.FileName,
                        FileContent = x.FileContent
                    }).FirstOrDefault();


                try
                {
                    string tempFilePath = System.IO.Path.GetTempFileName();

                    using (FileStream fileStream = new FileStream(convertedResult.FileName, FileMode.Create))
                    {
                        fileStream.Write(convertedResult.FileContent, 0, convertedResult.FileContent.Length);
                    }

                    // Открываем временный файл с использованием программы по умолчанию
                    System.Diagnostics.Process.Start(convertedResult.FileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка при открытии файла: {ex.Message}");
                }

                var vv = db.Chapter.Select(ch => ch.ChapterName).ToList();
            }
        }

        private void TakeTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // ConvertToIFC();
            string serveerpath = FolderName.Text;
            var root = ServerFiles1.First();
            root.Children.Clear();
            AddContents(ServerFiles1.FirstOrDefault(), serveerpath);
            
            
        }
        private void CreateTreeFiles()
        {
            ServerFiles1 = ServerFilesModel.CreateFiles();

            ServerFilesModel root = ServerFiles1[0];

            base.CommandBindings.Add(new CommandBinding(
                   ApplicationCommands.Undo, (sender, e) => // Execute
                   {
                       e.Handled = true;
                       root.IsChecked = false;
                       tree.Focus();
                   }, (sender, e) => // CanExecute
                   {
                       e.Handled = true;
                       e.CanExecute = (root.IsChecked != false);
                   }));
            tree.InvalidateVisual();
            tree.Focus();
        }
        private void ConvertToIFC()
        {
            string filePath = @"C:\Users\user\Documents\ConfigTest.json";
            string jsonContent = File.ReadAllText(filePath);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Transaction transaction = new Transaction(doc, "ExportIFC");
            try
            {
                object deserializedObject = serializer.DeserializeObject(jsonContent);
                var ttt = deserializedObject;
                IDictionary<string, object> dictionary = ToDictionary(deserializedObject);
                IFCBIM.IFCExportConfiguration myIFCExportConfiguration = IFCBIM.IFCExportConfiguration.CreateDefaultConfiguration();
                myIFCExportConfiguration.DeserializeFromJson(dictionary, serializer);

                var collector = new FilteredElementCollector(doc);
                var views = collector.OfCategory(BuiltInCategory.OST_Views)
                    .Where(v => v.Name == "Kitchen")
                    .FirstOrDefault();


                ElementId ExportViewId = views.Id;
                IFCExportOptions IFCExportOptions = new IFCExportOptions();
                myIFCExportConfiguration.UpdateOptions(IFCExportOptions, ExportViewId);
                string Directory = @"C:\Users\user\Documents\IFCEXPORT";

                transaction.Start();
                doc.Export(Directory, "export.ifc", IFCExportOptions);
                transaction.Commit();


            }
            catch
            {
                transaction.RollBack();
            }
        }
        private static IDictionary<string, object> ToDictionary(dynamic obj)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            if (obj is IDictionary<string, object> objDict)
            {
                foreach (var kvp in objDict)
                {
                    dictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return dictionary;
        }
        private XmlDictionaryReader GetResponseNew(string info)
        {
            // Create request
            try
            {
                //string stringt = stringt = $"http://RVTSRV2020-2.GGP.local/RevitServerAdminRESTService/AdminRESTService.svc/{info}";
                string stringt  = $"http://rvtsrv2022.ggp.local/RevitServerAdminRESTService{app.VersionNumber}/AdminRESTService.svc/{info}";

                WebRequest request = WebRequest.Create(stringt);
                request.Method = "GET";

                // Add the information the request needs

                request.Headers.Add("User-Name", Environment.UserName);
                request.Headers.Add("User-Machine-Name", Environment.MachineName);
                request.Headers.Add("Operation-GUID", Guid.NewGuid().ToString());

                // Read the response
                return JsonReaderWriterFactory.CreateJsonReader(request.GetResponse().GetResponseStream(), new XmlDictionaryReaderQuotas());
            }

            catch
            {
               return null;
            }
        }
       
        private async void AddContents(ServerFilesModel parentNode, string path)
        {
            
            var reader = await System.Threading.Tasks.Task.Run(() =>
            {
                return GetResponseNew(path + "/contents");
            });
            
            
            // Add the folders
            if (reader != null)
            {
                
                while (reader.Read())
                {
                    
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Folders")
                    {
                        while (reader.Read())
                        {
                            if (
                              reader.NodeType == XmlNodeType.EndElement &&
                              reader.Name == "Folders"
                            )
                                break;

                            if (
                              reader.NodeType == XmlNodeType.Element &&
                              reader.Name == "Name"
                            )
                            {
                                reader.Read();
                                ServerFilesModel node = new ServerFilesModel(reader.Value);
                                parentNode.Children.Add(node);
                                AddContents(node, path + "|" + reader.Value);
                            }
                        }
                    }
                    else if (
                      reader.NodeType == XmlNodeType.Element &&
                      reader.Name == "Models"
                    )
                    {
                        while (reader.Read())
                        {
                            if (
                              reader.NodeType == XmlNodeType.EndElement &&
                              reader.Name == "Models"
                            )
                                break;

                            if (
                              reader.NodeType == XmlNodeType.Element &&
                              reader.Name == "Name"
                            )
                            {
                                reader.Read();
                                ServerFilesModel node = new ServerFilesModel(reader.Value);
                                parentNode.Children.Add(node);

                            }
                        }
                    }
                    
                }

                // Close the reader 

                reader.Close();
                
            }
           






        }
    }
   
}
