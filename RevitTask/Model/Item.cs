using System.Collections.Generic;
using System.ComponentModel;

namespace RevitTask.Model
{
    internal class Item : INotifyPropertyChanged
    {
        private bool isChecked;
        public string Name { get; set; }
        public List<Item> Children { get; set; }
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
