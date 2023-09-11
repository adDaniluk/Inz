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
        public DbSet<DiseaseSuspicion> DiseaseSuspicions { get; set; }
        public DbSet<DoctorServices> DoctorServices { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TimeBlock> TimeBlocks { get; set; }
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

            modelBuilder.Entity<DiseaseSuspicion>()
                .HasKey(ds => new { ds.DoctorVisitId, ds.DiseaseId });

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalSpecializations)
                .WithMany(ms => ms.Doctors)
                .UsingEntity("DoctorMedicalSpecialization");

            modelBuilder.Entity<Doctor>()
                 .HasMany(d => d.CuredDiseases)
                 .WithMany(d => d.Doctors)
                 .UsingEntity("DoctorCuredDiseases");

            modelBuilder.Entity<DoctorServices>()
                .HasKey(ds => new {ds.ServiceId , ds.DoctorId });

            modelBuilder.Entity<ReceiptMedicine>()
                .HasKey(rm => new { rm.MedicineId, rm.ReceiptId });

            modelBuilder.Entity<Calendar>()
                .Property(c => c.Date)
                .HasColumnType("date");

            modelBuilder.Entity<Doctor>()
                .Property(d => d.DateOfBirth)
                .HasColumnType("date");

            modelBuilder.Entity<Patient>()
                .Property(d => d.DateOfBirth)
                .HasColumnType("date");
        }
    }
}
