using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                IdDoctor = 1,
                FirstName = "Anna",
                LastName = "Nowak",
                Email = "anna.nowak@clinic.com"
            },
            new Doctor
            {
                IdDoctor = 2,
                FirstName = "Marek",
                LastName = "Kowalski",
                Email = "marek.kowalski@clinic.com"
            }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient
            {
                IdPatient = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Birthdate = new DateTime(1990, 1, 1)
            },
            new Patient
            {
                IdPatient = 2,
                FirstName = "Ewa",
                LastName = "Nowak",
                Birthdate = new DateTime(1985, 6, 15)
            }
        );

        modelBuilder.Entity<Medicament>().HasData(
            new Medicament
            {
                IdMedicament = 1,
                Name = "Apap",
                Description = "Painkiller",
                Type = "Tablet"
            },
            new Medicament
            {
                IdMedicament = 2,
                Name = "Ibuprofen",
                Description = "Anti-inflammatory",
                Type = "Capsule"
            }
        );

        modelBuilder.Entity<Prescription>().HasData(
            new Prescription
            {
                IdPrescription = 1,
                Date = new DateTime(2024, 5, 1),
                DueDate = new DateTime(2024, 5, 10),
                IdDoctor = 1,
                IdPatient = 1
            },
            new Prescription
            {
                IdPrescription = 2,
                Date = new DateTime(2024, 5, 2),
                DueDate = new DateTime(2024, 5, 15),
                IdDoctor = 2,
                IdPatient = 2
            }
        );

        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament
            {
                IdMedicament = 1,
                IdPrescription = 1,
                Dose = 2,
                Description = "Take after meal"
            },
            new PrescriptionMedicament
            {
                IdMedicament = 2,
                IdPrescription = 1,
                Dose = 1,
                Description = "Once a day"
            },
            new PrescriptionMedicament
            {
                IdMedicament = 2,
                IdPrescription = 2,
                Dose = 3,
                Description = "Morning and evening"
            }
        );
    }
}
