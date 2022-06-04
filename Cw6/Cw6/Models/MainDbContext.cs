using Microsoft.EntityFrameworkCore;
using System;

namespace Cw6.Models
{
    public partial class MainDbContext : DbContext
    {
        public MainDbContext()
        {
        }

        public MainDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription_Medicament> Precsription_Medicaments { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //   optionsBuilder.UseSqlServer("Data Source=db-mssql16;Initial Catalog=2019SBD;Integrated Security=True");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Prescription_Medicament>().HasKey(e => new { e.IdPrescription, e.IdMedicament });

            modelBuilder.Entity<Prescription_Medicament>().Property(e => e.Details).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Prescription_Medicament>().HasOne(e => e.Prescription)
                .WithMany(m => m.Prescription_Medicaments)
                .HasForeignKey(m => m.IdPrescription)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription_Medicament>().HasOne(e => e.Medicament)
                .WithMany(m => m.Prescription_Medicaments)
                .HasForeignKey(m => m.IdMedicament)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor() { IdDoctor = 1, FirstName = "Andrzej", LastName = "Osobwski", Email = "osowand@chr.com" },
                new Doctor() { IdDoctor = 2, FirstName = "Wojciech", LastName = "Chojak", Email = "chojwoj@tmc@com" }
                );

            modelBuilder.Entity<Patient>().HasData(
                new Patient() { IdPatient = 1, FirstName = "Laura", LastName = "Ghina", Birthdate = DateTime.Now },
                new Patient() { IdPatient = 2, FirstName = "Jeremny", LastName = "Lovato", Birthdate = DateTime.Now }

                );

            modelBuilder.Entity<Prescription>().HasData(
                new Models.Prescription() { IdPrescription = 1, Date = DateTime.Now, DueDate = DateTime.Now, IdDoctor = 1, IdPatient = 1 },
                new Models.Prescription() { IdPrescription = 2, Date = DateTime.Now, DueDate = DateTime.Now, IdDoctor = 1, IdPatient = 2 }

                );

            modelBuilder.Entity<Medicament>().HasData(
                new Medicament() { IdMedicament = 1, Name = "Tetralysal", Description = "Lek na tradzik", Type = "Pigulki" }
                );
            modelBuilder.Entity<Prescription_Medicament>().HasData(
                new Prescription_Medicament() { IdMedicament = 1, IdPrescription = 1, Details = "llll" }
                );
        }
    }
}
