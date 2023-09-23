using Microsoft.EntityFrameworkCore;

namespace MasterDetailsUsingCoreJQAjax.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext()
        {
            
        }
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Designation>().HasData(
            new { DesignationId = 1, DesignationName = "Developer" },
            new { DesignationId = 2, DesignationName = "Sr.Developer" }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
