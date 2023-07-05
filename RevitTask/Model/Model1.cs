using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace RevitTask.Model
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base(@"Server=MARINA\SQLEXPRESS; Database = RevitDB; Trusted_Connection =True;")
        {
        }

        public virtual DbSet<AK> AK { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskFiles> TaskFiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AK>()
                .Property(e => e.FamilyType)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.FamilyName)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.GGP_Задание)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.ADSK_Марка)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.ADSK_Напряжение)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.ADSK_Номинальная_мощность)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.GGP_Тип_системы)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.ADSK_Количество_фаз)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.ADSK_Коэффицент_мощности)
                .IsUnicode(false);

            modelBuilder.Entity<AK>()
                .Property(e => e.GGP_Примечание_МногострочныйТекст)
                .IsUnicode(false);

            modelBuilder.Entity<Chapter>()
                .Property(e => e.ChapterName)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.TaskTimeTastamp)
                .IsFixedLength();

            modelBuilder.Entity<Task>()
                .Property(e => e.Comment)
                .IsUnicode(false);

           
            modelBuilder.Entity<TaskFiles>()
                .Property(e => e.FileName)
                .IsUnicode(false);
        }
    }
}
