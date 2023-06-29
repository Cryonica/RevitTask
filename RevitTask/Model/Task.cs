namespace RevitTask.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Task : INotifyPropertyChanged
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        
        public Task()
        {
            AK = new HashSet<AK>();
        }
        
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private int? chapterId;
        public int? ChapterId
        {
            get { return chapterId; }
            set
            {
                if (chapterId != value)
                {
                    chapterId = value;
                    OnPropertyChanged(nameof(ChapterId));
                }
            }
        }
        private bool hasAttachments;
        public bool HasAttachments
        {
            get { return hasAttachments; }
            set
            {
                if (hasAttachments != value)
                {
                    hasAttachments = value;
                    OnPropertyChanged(nameof(HasAttachments));
                }
            }
        }

        private DateTime? taskTime;
        public DateTime? TaskTime
        {
            get { return taskTime; }
            set
            {
                if (taskTime != value)
                {
                    taskTime = value;
                    OnPropertyChanged(nameof(TaskTime));
                }
            }
        }

        private byte[] taskTimeTastamp;
        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TaskTimeTastamp
        {
            get { return taskTimeTastamp; }
            set
            {
                if (taskTimeTastamp != value)
                {
                    taskTimeTastamp = value;
                    OnPropertyChanged(nameof(TaskTimeTastamp));
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AK> AK { get; set; }

        public virtual Chapter Chapter { get; set; }

        internal event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
