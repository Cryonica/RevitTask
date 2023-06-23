using RevitTask.Model;
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

namespace RevitTask
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
       
        public MainForm()
        {
            InitializeComponent();
            button.Click += Button_Click;
            List<Item> items = new List<Item>
            {
                new Item { 
                    Name = "Item 1", 
                    Children = new List<Item> { 
                        new Item { 
                            Name = "Subitem 1.1" 
                        }, 
                        new Item { 
                            Name = "Subitem 1.2" 
                        } } },
                new Item { 
                    Name = "Item 2", 
                    Children = new List<Item> { 
                        new Item { 
                            Name = "Subitem 2.1" 
                        },
                        new Item {
                            Name = "Subitem 2.2" } } },
                new Item { Name = "Item 3" }
            };

            treeView.ItemsSource = items;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Item> selectedItems = GetSelectedItems();

            MessageBox.Show("Selected Items: " + string.Join(", ", selectedItems.Select(si=> si.Name)));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateChildItems((CheckBox)sender, true);
            UpdateParentItems((CheckBox)sender);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateChildItems((CheckBox)sender, false);
            UpdateParentItems((CheckBox)sender);
        }
        private void UpdateChildItems(CheckBox checkBox, bool isChecked)
        {
            if (checkBox.DataContext is Item item)
            {
                if (item.Children?.Count >0 )
                {
                    foreach (Item childItem in item.Children)
                    {
                        childItem.IsChecked = isChecked;
                        UpdateChildItemsRecursive(childItem, isChecked);
                    }

                }
                
            }
        }
        private void UpdateChildItemsRecursive(Item item, bool isChecked)
        {
            if (item.Children != null)
            {
                foreach (Item childItem in item.Children)
                {
                    childItem.IsChecked = isChecked;
                    UpdateChildItemsRecursive(childItem, isChecked);
                }

            }
            
        }

        private void UpdateParentItemsRecursive(Item item)
        {
            if (treeView.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement itemContainer && itemContainer.Parent is TreeViewItem parentItemContainer && parentItemContainer.DataContext is Item parentItem)
            {
                bool allChildrenChecked = true;
                bool anyChildChecked = false;

                foreach (Item childItem in parentItem.Children)
                {
                    if (childItem.IsChecked)
                        anyChildChecked = true;
                    else
                        allChildrenChecked = false;
                }

                parentItem.IsChecked = allChildrenChecked;
                UpdateParentItemsRecursive(parentItem);

                if (anyChildChecked)
                    parentItemContainer.IsExpanded = true;
            }
        }
        private void UpdateParentItems(CheckBox checkBox)
        {
            bool allChildrenChecked = true;
            bool anyChildChecked = false;
            if (checkBox.DataContext is Item item && treeView.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement itemContainer)
            {
                if (itemContainer.Parent is TreeViewItem parentItemContainer && parentItemContainer.DataContext is Item parentItem)
                {
                    foreach (Item childItem in parentItem.Children)
                    {
                        if (childItem.IsChecked)
                            anyChildChecked = true;
                        else
                            allChildrenChecked = false;
                    }

                    parentItem.IsChecked = allChildrenChecked;
                    UpdateParentItemsRecursive(parentItem);

                    if (anyChildChecked)
                        parentItemContainer.IsExpanded = true;
                }
            }
        }
        private List<Item> GetSelectedItems()
        {
            List<Item> selectedItems = new List<Item>();

            foreach (Item item in treeView.Items)
            {
                CollectSelectedItemsRecursive(item, selectedItems);
            }

            return selectedItems;
        }
        private void CollectSelectedItemsRecursive(Item item, List<Item> selectedItems)
        {
            if (item.IsChecked)
            {
                selectedItems.Add(item);
            }

            if (item.Children != null)
            {
                foreach (Item childItem in item.Children)
                {
                    CollectSelectedItemsRecursive(childItem, selectedItems);
                }

            }
            
        }

        private TreeViewItem FindTreeViewItemFromMousePosition(Visual visual, Point position)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(visual, position);
            if (hitTestResult != null)
            {
                DependencyObject dependencyObject = hitTestResult.VisualHit;
                while (dependencyObject != null && !(dependencyObject is TreeViewItem))
                {
                    dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                }

                return dependencyObject as TreeViewItem;
            }

            return null;
        }
        private IEnumerable<Item> GetItems(TreeView treeView)
        {
            List<Item> items = new List<Item>();

            foreach (Item item in treeView.Items)
            {
                items.Add(item);
                CollectItemsRecursive(item, items);
            }

            return items;
        }

        private void treeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isShiftKeyPressed)
            {
                TreeView treeView = (TreeView)sender;
                TreeViewItem treeViewItem = FindTreeViewItemFromMousePosition(treeView, e.GetPosition(treeView));
                if (treeViewItem != null && treeViewItem.DataContext is Item item)
                {
                    if (startSelectedItem == null)
                    {
                        startSelectedItem = item;
                    }
                    else
                    {
                        bool select = false;

                        foreach (Item childItem in GetItems(treeView))
                        {
                            if (childItem == item || childItem == startSelectedItem)
                            {
                                select = !select;
                            }

                            if (select)
                            {
                                childItem.IsChecked = true;
                            }
                        }

                        startSelectedItem = null;
                    }
                }
            }
        }
    }
   
}
