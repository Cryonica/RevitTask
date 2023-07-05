using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitTask.Model
{
    public class ServerFilesModel : INotifyPropertyChanged
    {
        bool? _isChecked = false;
        ServerFilesModel _parent;

        public bool IsInitiallySelected { get; private set; }
        public string Name { get; private set; }
        public ObservableCollection<ServerFilesModel> Children { get; private set; }
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { this.SetIsChecked(value, true, true); }
        }

        public ServerFilesModel(string name)
        {
            Name = name;
            Children = new ObservableCollection<ServerFilesModel>();
            Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Children));
        }
        public void SetRootName(string name)
        {
            Name = name;
            OnPropertyChanged(nameof(Name));
        }

        public static ObservableCollection<ServerFilesModel> CreateFiles()
        {
            ServerFilesModel root = new ServerFilesModel("Server")
            {
                IsInitiallySelected = true,
                //Children =
                //{
                //    new ServerFilesModel("file1")
                //    {
                //        Children =
                //        {
                //            new ServerFilesModel("file1.1"),
                //            new ServerFilesModel("file1.2"),

                //        }
                //    },
                //    new ServerFilesModel("file2")
                //    {
                //        Children =
                //        {
                //            new ServerFilesModel("file2.1"),
                //            new ServerFilesModel("file2.2"),

                //        }
                //    }
                //}
            };
            root.Initialize();

           
            return new ObservableCollection<ServerFilesModel> { root };
        }
        void Initialize()
        {
            foreach (ServerFilesModel child in this.Children)
            {
                child._parent = this;
                child.Initialize();
            }
           
        }
        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
            {
                this.Children.ToList()
                .ForEach(c => c.SetIsChecked(_isChecked, true, false));
            }

            if (updateParent && _parent != null)
                _parent.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }
        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }



        void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
