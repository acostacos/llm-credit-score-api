using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using Microsoft.EntityFrameworkCore;

namespace llm_credit_score_api.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<AppTask> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("company_metadata");
            modelBuilder.Entity<AppTask>().ToTable("tasks");
        }
    }
}
