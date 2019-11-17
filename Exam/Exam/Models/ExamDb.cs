namespace Exam.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ExamDb : DbContext
    {
        public ExamDb()
            : base("name=ExamDb")
        {
        }

        public virtual DbSet<category> categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.categories1)
                .WithOptional(e => e.category1)
                .HasForeignKey(e => e.ParentId);
        }
    }
}
