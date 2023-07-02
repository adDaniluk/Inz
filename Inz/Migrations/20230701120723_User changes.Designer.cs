﻿// <auto-generated />
using System;
using Inz.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inz.Migrations
{
    [DbContext(typeof(DbContextApi))]
    [Migration("20230701120723_User changes")]
    partial class Userchanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inz.Model.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AlterTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("AparmentNumber")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("DeleteTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Inz.Model.Calendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AlterTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeleteTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("DoctorVisitId")
                        .HasColumnType("int");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.Property<int?>("ServicePrice")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("TimeBlockId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TimeBlockId");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("Inz.Model.CuredDisease", b =>
                {
                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("DiseaseId")
                        .HasColumnType("int");

                    b.HasKey("DoctorId", "DiseaseId");

                    b.HasIndex("DiseaseId");

                    b.ToTable("CuredDiseases");
                });

            modelBuilder.Entity("Inz.Model.Disease", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Diseases");
                });

            modelBuilder.Entity("Inz.Model.DiseaseSuspicion", b =>
                {
                    b.Property<int>("DoctorVisitId")
                        .HasColumnType("int");

                    b.Property<int>("DiseaseId")
                        .HasColumnType("int");

                    b.HasKey("DoctorVisitId", "DiseaseId");

                    b.HasIndex("DiseaseId");

                    b.ToTable("DiseaseSuspicions");
                });

            modelBuilder.Entity("Inz.Model.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AlterTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("Date");

                    b.Property<DateTime?>("DeleteTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<int>("LicenseNumber")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Inz.Model.DoctorMedicalSpecialization", b =>
                {
                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("MedicalSpecializationId")
                        .HasColumnType("int");

                    b.HasKey("DoctorId", "MedicalSpecializationId");

                    b.HasIndex("MedicalSpecializationId");

                    b.ToTable("DoctorMedicalSpecializations");
                });

            modelBuilder.Entity("Inz.Model.DoctorService", b =>
                {
                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("ServiceId", "DoctorId");

                    b.HasIndex("DoctorId");

                    b.ToTable("DoctorServices");
                });

            modelBuilder.Entity("Inz.Model.DoctorVisit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdditionalCosts")
                        .HasColumnType("int");

                    b.Property<int>("CalendarId")
                        .HasColumnType("int");

                    b.Property<string>("EndHour")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaymentTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("RatingNote")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId")
                        .IsUnique();

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("DoctorVisits");
                });

            modelBuilder.Entity("Inz.Model.MedicalSpecialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("MedicalSpecializations");
                });

            modelBuilder.Entity("Inz.Model.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("Inz.Model.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AlterTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("Date");

                    b.Property<DateTime?>("DeleteTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Inz.Model.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("Inz.Model.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DoctorVisitId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DoctorVisitId")
                        .IsUnique();

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("Inz.Model.ReceiptMedicine", b =>
                {
                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<int>("ReceiptId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("MedicineId", "ReceiptId");

                    b.HasIndex("ReceiptId");

                    b.ToTable("ReceiptMedicines");
                });

            modelBuilder.Entity("Inz.Model.Referral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("DoctorVisitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DoctorVisitId");

                    b.ToTable("Referrals");
                });

            modelBuilder.Entity("Inz.Model.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Inz.Model.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("Inz.Model.TimeBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EndHour")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("StartHour")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("TimeBlocks");
                });

            modelBuilder.Entity("Inz.Model.Calendar", b =>
                {
                    b.HasOne("Inz.Model.Doctor", "Doctor")
                        .WithMany("Calendars")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.Patient", "Patient")
                        .WithMany("Calendars")
                        .HasForeignKey("PatientId");

                    b.HasOne("Inz.Model.Service", "Service")
                        .WithMany("Calendars")
                        .HasForeignKey("ServiceId");

                    b.HasOne("Inz.Model.Status", "Status")
                        .WithMany("Calendars")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.TimeBlock", "TimeBlock")
                        .WithMany("Calendars")
                        .HasForeignKey("TimeBlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");

                    b.Navigation("Service");

                    b.Navigation("Status");

                    b.Navigation("TimeBlock");
                });

            modelBuilder.Entity("Inz.Model.CuredDisease", b =>
                {
                    b.HasOne("Inz.Model.Disease", "Disease")
                        .WithMany("CuredDiseases")
                        .HasForeignKey("DiseaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.Doctor", "Doctor")
                        .WithMany("CuredDiseases")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disease");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Inz.Model.DiseaseSuspicion", b =>
                {
                    b.HasOne("Inz.Model.Disease", "Disease")
                        .WithMany("DiseaseSuspicions")
                        .HasForeignKey("DiseaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.DoctorVisit", "DoctorVisit")
                        .WithMany("DiseaseSuspicions")
                        .HasForeignKey("DoctorVisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disease");

                    b.Navigation("DoctorVisit");
                });

            modelBuilder.Entity("Inz.Model.Doctor", b =>
                {
                    b.HasOne("Inz.Model.Address", "Address")
                        .WithOne("Doctor")
                        .HasForeignKey("Inz.Model.Doctor", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Inz.Model.DoctorMedicalSpecialization", b =>
                {
                    b.HasOne("Inz.Model.Doctor", "Doctor")
                        .WithMany("DoctorMedicalSpecializations")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.MedicalSpecialization", "MedicalSpecialization")
                        .WithMany("DoctorMedicalSpecializations")
                        .HasForeignKey("MedicalSpecializationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("MedicalSpecialization");
                });

            modelBuilder.Entity("Inz.Model.DoctorService", b =>
                {
                    b.HasOne("Inz.Model.Doctor", "Doctor")
                        .WithMany("DoctorServices")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.Service", "Service")
                        .WithMany("DoctorServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Inz.Model.DoctorVisit", b =>
                {
                    b.HasOne("Inz.Model.Calendar", "Calendar")
                        .WithOne("DoctorVisit")
                        .HasForeignKey("Inz.Model.DoctorVisit", "CalendarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.PaymentType", "PaymentType")
                        .WithMany("DoctorVisits")
                        .HasForeignKey("PaymentTypeId");

                    b.Navigation("Calendar");

                    b.Navigation("PaymentType");
                });

            modelBuilder.Entity("Inz.Model.Patient", b =>
                {
                    b.HasOne("Inz.Model.Address", "Address")
                        .WithOne("Patient")
                        .HasForeignKey("Inz.Model.Patient", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Inz.Model.Receipt", b =>
                {
                    b.HasOne("Inz.Model.DoctorVisit", "DoctorVisit")
                        .WithOne("Receipt")
                        .HasForeignKey("Inz.Model.Receipt", "DoctorVisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoctorVisit");
                });

            modelBuilder.Entity("Inz.Model.ReceiptMedicine", b =>
                {
                    b.HasOne("Inz.Model.Medicine", "Medicine")
                        .WithMany("ReceiptMedicines")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inz.Model.Receipt", "Receipt")
                        .WithMany("ReceiptMedicines")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Inz.Model.Referral", b =>
                {
                    b.HasOne("Inz.Model.DoctorVisit", "DoctorVisit")
                        .WithMany("Referrals")
                        .HasForeignKey("DoctorVisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoctorVisit");
                });

            modelBuilder.Entity("Inz.Model.Address", b =>
                {
                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Inz.Model.Calendar", b =>
                {
                    b.Navigation("DoctorVisit");
                });

            modelBuilder.Entity("Inz.Model.Disease", b =>
                {
                    b.Navigation("CuredDiseases");

                    b.Navigation("DiseaseSuspicions");
                });

            modelBuilder.Entity("Inz.Model.Doctor", b =>
                {
                    b.Navigation("Calendars");

                    b.Navigation("CuredDiseases");

                    b.Navigation("DoctorMedicalSpecializations");

                    b.Navigation("DoctorServices");
                });

            modelBuilder.Entity("Inz.Model.DoctorVisit", b =>
                {
                    b.Navigation("DiseaseSuspicions");

                    b.Navigation("Receipt");

                    b.Navigation("Referrals");
                });

            modelBuilder.Entity("Inz.Model.MedicalSpecialization", b =>
                {
                    b.Navigation("DoctorMedicalSpecializations");
                });

            modelBuilder.Entity("Inz.Model.Medicine", b =>
                {
                    b.Navigation("ReceiptMedicines");
                });

            modelBuilder.Entity("Inz.Model.Patient", b =>
                {
                    b.Navigation("Calendars");
                });

            modelBuilder.Entity("Inz.Model.PaymentType", b =>
                {
                    b.Navigation("DoctorVisits");
                });

            modelBuilder.Entity("Inz.Model.Receipt", b =>
                {
                    b.Navigation("ReceiptMedicines");
                });

            modelBuilder.Entity("Inz.Model.Service", b =>
                {
                    b.Navigation("Calendars");

                    b.Navigation("DoctorServices");
                });

            modelBuilder.Entity("Inz.Model.Status", b =>
                {
                    b.Navigation("Calendars");
                });

            modelBuilder.Entity("Inz.Model.TimeBlock", b =>
                {
                    b.Navigation("Calendars");
                });
#pragma warning restore 612, 618
        }
    }
}
