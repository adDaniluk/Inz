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
        public DbSet<DoctorService> DoctorServices { get; set; }
        public DbSet<Disease> Diseases { get; set; }
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
                .WithOne(ds => ds.Doctor);
        
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Address)
                .WithOne(a => a.Patient);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Calendars)
                .WithOne(c => c.Patient);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.DoctorMedicalSpecializations)
                .WithOne(dms => dms.Doctor);

            modelBuilder.Entity<Status>()
                .HasMany(s => s.Calendars)
                .WithOne(c => c.Status);

            modelBuilder.Entity<Service>()
                .HasMany(s => s.DoctorServices)
                .WithOne(ds => ds.Service);

            modelBuilder.Entity<MedicalSpecialization>()
                .HasMany(ms => ms.DoctorMedicalSpecializations)
                .WithOne(dms => dms.MedicalSpecialization);

            modelBuilder.Entity<Service>()
                .HasMany(s => s.Calendars)
                .WithOne(c => c.Service);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.CuredDiseases)
                .WithOne(cd => cd.Doctor);

            modelBuilder.Entity<Disease>()
                .HasMany(d => d.CuredDiseases)
                .WithOne(cd => cd.Disease);

            modelBuilder.Entity<Disease>()
                .HasMany(d => d.DiseaseSuspicions)
                .WithOne(ds => ds.Disease);

            modelBuilder.Entity<DoctorVisit>()
                .HasMany(dv => dv.DiseaseSuspicions)
                .WithOne(ds => ds.DoctorVisit);

            modelBuilder.Entity<DoctorVisit>()
                .HasMany(dv => dv.Referrals)
                .WithOne(r => r.DoctorVisit);

            modelBuilder.Entity<PaymentType>()
                .HasMany(pt => pt.DoctorVisits)
                .WithOne(dv => dv.PaymentType);

            modelBuilder.Entity<TimeBlock>()
                .HasMany(tb => tb.Calendars)
                .WithOne(c => c.TimeBlock);

            modelBuilder.Entity<Medicine>()
                .HasMany(m => m.ReceiptMedicines)
                .WithOne(rm => rm.Medicine);

            modelBuilder.Entity<Receipt>()
                .HasMany(r => r.ReceiptMedicines)
                .WithOne(rm => rm.Receipt);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.DoctorVisit)
                .WithOne(dv => dv.Receipt);

            modelBuilder.Entity<Calendar>()
                .HasOne(c => c.DoctorVisit)
                .WithOne(dv => dv.Calendar)
                .HasForeignKey<DoctorVisit>(dv => dv.CalendarId);

            modelBuilder.Entity<CuredDisease>()
                .HasKey(cd => new { cd.DoctorId, cd.DiseaseId });

            modelBuilder.Entity<DiseaseSuspicion>()
                .HasKey(ds => new { ds.DoctorVisitId, ds.DiseaseId });

            modelBuilder.Entity<DoctorMedicalSpecialization>()
                .HasKey(dms => new { dms.DoctorId, dms.MedicalSpecializationId });

            modelBuilder.Entity<DoctorService>()
                .HasKey(ds => new {ds.ServiceId , ds.DoctorId });

            modelBuilder.Entity<ReceiptMedicine>()
                .HasKey(rm => new { rm.MedicineId, rm.ReceiptId });

//Modlbuilder Annotations

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .Property(a => a.PostCode)
                .HasMaxLength(6);

            modelBuilder.Entity<Address>()
                .Property(a => a.PostCode)
                .HasMaxLength(6);

            modelBuilder.Entity<Disease>()
                .Property(d => d.Name)
                .HasMaxLength(200);

            modelBuilder.Entity<DoctorVisit>()
                .Property(dv => dv.EndHour)
                .HasMaxLength(10);

            modelBuilder.Entity<DoctorVisit>()
                .Property(dv => dv.RatingNote)
                .HasMaxLength(200);

            modelBuilder.Entity<MedicalSpecialization>()
                .Property(ms => ms.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Medicine>()
                .Property(m => m.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<PaymentType>()
               .Property(pt => pt.TypeName)
               .HasMaxLength(100);

            modelBuilder.Entity<Service>()
               .Property(s => s.Description)
               .HasMaxLength(200);

            modelBuilder.Entity<Service>()
               .Property(s => s.Name)
               .HasMaxLength(200);

            modelBuilder.Entity<Status>()
               .Property(s => s.StatusName)
               .HasMaxLength(100);

            modelBuilder.Entity<TimeBlock>()
               .Property(tb => tb.StartHour)
               .HasMaxLength(10);

            modelBuilder.Entity<TimeBlock>()
              .Property(tb => tb.EndHour)
              .HasMaxLength(10);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Login)
                .HasMaxLength(50);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Password)
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Phone)
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Name)
                .HasMaxLength(200);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Surname)
                .HasMaxLength(200);

            modelBuilder.Entity<Patient>()
                .Property(d => d.Login)
                .HasMaxLength(50);

            modelBuilder.Entity<Patient>()
                .Property(d => d.Password)
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(d => d.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(d => d.Phone)
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(d => d.Name)
                .HasMaxLength(200);

            modelBuilder.Entity<Patient>()
                .Property(d => d.Surname)
                .HasMaxLength(200);
        }
    }
}
