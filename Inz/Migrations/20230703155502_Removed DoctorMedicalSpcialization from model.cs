using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inz.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDoctorMedicalSpcializationfrommodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorMedicalSpecializations");

            migrationBuilder.DropColumn(
                name: "AlterTimestamp",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DeleteTimestamp",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Addresses");

            migrationBuilder.CreateTable(
                name: "DoctorMedicalSpecialization",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    MedicalSpecializationId = table.Column<int>(type: "int", nullable: false),
                    DoctorsId = table.Column<int>(type: "int", nullable: false),
                    MedicalSpecializationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorMedicalSpecialization", x => new { x.DoctorId, x.MedicalSpecializationId });
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecialization_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecialization_Doctors_DoctorsId",
                        column: x => x.DoctorsId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecialization_MedicalSpecializations_MedicalSpecializationId",
                        column: x => x.MedicalSpecializationId,
                        principalTable: "MedicalSpecializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecialization_MedicalSpecializations_MedicalSpecializationsId",
                        column: x => x.MedicalSpecializationsId,
                        principalTable: "MedicalSpecializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalSpecialization_DoctorsId",
                table: "DoctorMedicalSpecialization",
                column: "DoctorsId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalSpecialization_MedicalSpecializationId",
                table: "DoctorMedicalSpecialization",
                column: "MedicalSpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalSpecialization_MedicalSpecializationsId",
                table: "DoctorMedicalSpecialization",
                column: "MedicalSpecializationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorMedicalSpecialization");

            migrationBuilder.AddColumn<DateTime>(
                name: "AlterTimestamp",
                table: "Addresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTimestamp",
                table: "Addresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Addresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DoctorMedicalSpecializations",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    MedicalSpecializationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorMedicalSpecializations", x => new { x.DoctorId, x.MedicalSpecializationId });
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecializations_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecializations_MedicalSpecializations_MedicalSpecializationId",
                        column: x => x.MedicalSpecializationId,
                        principalTable: "MedicalSpecializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalSpecializations_MedicalSpecializationId",
                table: "DoctorMedicalSpecializations",
                column: "MedicalSpecializationId");
        }
    }
}
