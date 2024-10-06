using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using Microsoft.EntityFrameworkCore;

namespace llm_credit_score_api.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<FinancialRatio> FinancialRatios { get; set; }
        public DbSet<AppTask> Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("company_metadata");
            modelBuilder.Entity<FinancialRatio>().ToTable("company_financial_ratios");
            modelBuilder.Entity<AppTask>().ToTable("tasks");
            modelBuilder.Entity<Report>().ToTable("reports");

            modelBuilder.Entity<FinancialRatio>()
                .HasOne(e => e.Company)
                .WithMany(e => e.FinancialRatios)
                .HasForeignKey(e => e.CompanyId)
                .IsRequired(false);
            modelBuilder.Entity<Report>()
                .HasOne(e => e.Company)
                .WithMany(e => e.Reports)
                .HasForeignKey(e => e.CompanyId);
            modelBuilder.Entity<Report>()
                .HasOne(e => e.Task)
                .WithOne(e => e.Report)
                .HasForeignKey<Report>(e => e.TaskId)
                .IsRequired(false);
        }
    }
}
