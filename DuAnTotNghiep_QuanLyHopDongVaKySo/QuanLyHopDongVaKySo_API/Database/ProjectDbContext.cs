using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ModelProcedure;

namespace QuanLyHopDongVaKySo_API.Database
{
    public class ProjectDbContext:DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DoneContract> DoneContracts { get; set; }
        public DbSet<DoneMinute> DoneMinutes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<InstallationRequirement> InstallationRequirements { get; set; }
        public DbSet<InstallationDevice> InstallationDevices { get; set; }

        public DbSet<OperationHistoryEmp> OperationHistoryEmp { get; set; }
        public DbSet<OperationHistoryCus> OperationHistoryCus { get; set; }
        public DbSet<PendingContract> PendingContracts { get; set; }
        public DbSet<PendingMinute> PendingMinutes { get; set; }
        public DbSet<PFXCertificate> PFXCertificates { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TemplateContract> TemplateContracts { get; set; }
        public DbSet<TemplateMinute> TemplateMinutes { get; set; }
        public DbSet<TypeOfService> TypeOfServices { get; set; }
        public DbSet<ContractCoordinate> ContractCoordinates { get; set; }
        public DbSet<MinuteCoordinate> MinuteCoordinates { get; set; }
        public DbSet<Stamp> Stamps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<DoneContract>().ToTable("DoneContract");
            modelBuilder.Entity<DoneMinute>().ToTable("DoneMinute");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<InstallationRequirement>().ToTable("InstallationRequirement");
            modelBuilder.Entity<OperationHistoryEmp>().ToTable(tb => tb.HasTrigger("tr_DeleteOldEmpHistoryRecords"));
            modelBuilder.Entity<OperationHistoryCus>().ToTable(tb => tb.HasTrigger("tr_DeleteOldCusHistoryRecords"));
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<PendingContract>().ToTable("PendingContract");
            modelBuilder.Entity<PendingMinute>().ToTable("PendingMinute");
            modelBuilder.Entity<PFXCertificate>().ToTable("PFXCertificate");
            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<TemplateContract>().ToTable("TemplateContract");
            modelBuilder.Entity<TemplateMinute>().ToTable("TemplateMinute");
            modelBuilder.Entity<TypeOfService>().ToTable("TypeOfService");
            modelBuilder.Entity<ContractCoordinate>().ToTable("ContractCoordinate");
            modelBuilder.Entity<MinuteCoordinate>().ToTable("MinuteCoordinate");
            modelBuilder.Entity<InstallationDevice>().ToTable("InstallationDevice");
            modelBuilder.Entity<Stamp>().ToTable("Stamp");
        }
    }
}
