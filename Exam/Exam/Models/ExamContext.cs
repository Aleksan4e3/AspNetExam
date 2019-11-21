namespace Exam.Models
{
    using log4net;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class ExamContext : DbContext
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExamContext));

        public ExamContext()
            : base("name=ExamContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.SubCategories)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentId);
        }

        public override Task<int> SaveChangesAsync()
        {
            var addedEntities = ChangeTracker.Entries<Category>()
                .Where(c => c.State == EntityState.Added)
                .Select(c => c.Entity);

            var modifiedEntities = ChangeTracker.Entries<Category>()
                .Where(c => c.State == EntityState.Modified)
                .Select(c => c.Entity);

            var deletedEntities = ChangeTracker.Entries<Category>()
                .Where(c => c.State == EntityState.Deleted)
                .Select(c => c.Entity);

            foreach (var added in addedEntities)
            {
                Log.Info($"Added category {{Id = {added.Id}, Name = {added.Name}, ParentId = {added.ParentId}}}");
            }

            foreach (var modified in modifiedEntities)
            {
                Log.Info($"Modified category {{Id = {modified.Id}, Name = {modified.Name}, ParentId = {modified.ParentId}}}");
            }

            foreach (var deleted in deletedEntities)
            {
                Log.Info($"Deleted category {{Id = {deleted.Id}, Name = {deleted.Name}, ParentId = {deleted.ParentId}}}");
            }
            return base.SaveChangesAsync();
        }
    }
}
