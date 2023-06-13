using Inz.Model;
using Microsoft.EntityFrameworkCore;

namespace Inz.Context
{
    public class DbContextApi: DbContext
    {
        
        public DbContextApi(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<DoctorService> DoctorServices { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TimeBlock> TimeBlocks { get; set; }
        public DbSet<CuredDisease> CuredDiseases { get; set; }
        public DbSet<DoctorMedicalSpecialization> DoctorMedicalSpecializations { get; set; }
        public DbSet<DoctorVisit> DoctorVisits { get; set; }
        public DbSet<MedicalSpecialization> MedicalSpecializations { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptMedicine> ReceiptMedicines { get; set; }
        public DbSet<Referral> Referrals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Address)
                .WithOne(d => d.Doctor);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Calendars)
                .WithOne(d => d.Doctor);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.DoctorServices)
                .WithOne(d => d.Doctor);
        
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Address)
                .WithOne(p => p.Patient);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Calendars)
                .WithOne(p => p.Patient);

            modelBuilder.Entity<Status>()
                .HasMany(s => s.Calendars)
                .WithOne(s => s.Status);

            modelBuilder.Entity<Service>()
                .HasMany(s => s.DoctorServices)
                .WithOne(s => s.Service);
        }
    }
}
