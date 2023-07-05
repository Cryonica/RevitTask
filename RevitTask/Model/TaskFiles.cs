namespace RevitTask.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TaskFiles
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        [StringLength(64)]
        public string SHA256 { get; set; }

        [Required]
        public byte[] FileContent { get; set; }

        public virtual Task Task { get; set; }
        public int? TaskId { get; set; }

        public int Id { get; set; }

        
    }
}
