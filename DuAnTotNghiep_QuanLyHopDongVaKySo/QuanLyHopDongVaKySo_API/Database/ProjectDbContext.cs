using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Database
{
    public class ProjectDbContext:DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DoneContract> DoneContracts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PendingMinute> PendingMinutes { get; set; }
        public DbSet<InstallationRequirement> InstallationRequirements { get; set; }
        public DbSet<OperationHistory> OperationHistories { get; set; }
        public DbSet<PendingContract> PendingContracts { get; set; }
        public DbSet<PFXCertificate> PFXCertificates { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TemplateContract> TemplateContracts { get; set; }
        public DbSet<TemplateMinute> TemplateMinutes { get; set; }
        public DbSet<TypeOfCustomer> TypeOfCustomers { get; set; }
        public DbSet<TypeOfService> TypeOfServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<DoneContract>().ToTable("DoneContract");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<PendingMinute>().ToTable("PendingMinute");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<InstallationRequirement>().ToTable("InstallationRequirement");
            modelBuilder.Entity<OperationHistory>().ToTable("OperationHistory");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<PendingContract>().ToTable("PendingContract");
            modelBuilder.Entity<PFXCertificate>().ToTable("PFXCertificate");
            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<TemplateContract>().ToTable("TemplateContract");
            modelBuilder.Entity<TemplateMinute>().ToTable("TemplateMinute");
            modelBuilder.Entity<TypeOfCustomer>().ToTable("TypeOfCustomer");
            modelBuilder.Entity<TypeOfService>().ToTable("TypeOfService");
        }

    }
}
