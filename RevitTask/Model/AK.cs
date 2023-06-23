namespace RevitTask.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AK")]
    public partial class AK
    {
        public int Id { get; set; }

        public int? ChapterId { get; set; }

        [StringLength(50)]
        public string FamilyType { get; set; }

        [StringLength(50)]
        public string FamilyName { get; set; }

        public int FamilyId { get; set; }

        public int? TaskId { get; set; }

        [Required]
        [StringLength(50)]
        public string GGP_Задание { get; set; }

        [Required]
        [StringLength(50)]
        public string ADSK_Марка { get; set; }

        [Required]
        [StringLength(50)]
        public string ADSK_Напряжение { get; set; }

        [Required]
        [StringLength(50)]
        public string ADSK_Номинальная_мощность { get; set; }

        [StringLength(50)]
        public string GGP_Тип_системы { get; set; }

        [StringLength(50)]
        public string ADSK_Количество_фаз { get; set; }

        [StringLength(50)]
        public string ADSK_Коэффицент_мощности { get; set; }

        [Column(TypeName = "text")]
        public string GGP_Примечание_МногострочныйТекст { get; set; }

        public virtual Chapter Chapter { get; set; }

        public virtual Task Task { get; set; }
    }
}
