using Cw6.Models;
using Cw6.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw6.Services
{
    public class DbService : IDbService
    {
        private readonly MainDbContext _context;
        public DbService(MainDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DoctorGetter>> GetDoctors()
        {
            return await _context.Doctors
                .Select(e => new DoctorGetter
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                }) .ToListAsync();
        }
        public async Task<bool> AddDoctor(Doctor doctorToAdd)
        {
            var doctorExist = _context.Doctors.Where(e => e.FirstName == doctorToAdd.FirstName && e.LastName == doctorToAdd.LastName && e.Email == doctorToAdd.Email).FirstOrDefault();
            if (doctorExist == null) {
                var doctor = new Doctor()
                {
                    FirstName = doctorToAdd.FirstName,
                    LastName = doctorToAdd.LastName,
                    Email = doctorToAdd.Email
                };
                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<int> RemoveDoctor(int idDoctor)
        {
            var doctorExists = _context.Prescriptions.Where(e => e.IdDoctor == idDoctor).FirstOrDefault();
            if (doctorExists != null) {
                var doctor = _context.Doctors.Where(e => e.IdDoctor == idDoctor).FirstOrDefault();
                if (doctor != null)
                {
                    _context.Doctors.Remove(doctor);
                    await _context.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            return 2;
        }
        public async Task<bool> ModifyDoctor(DoctorGetter doctorToModify, int idDoctor)
        {
            var doctor = _context.Doctors.Where(e => e.IdDoctor == idDoctor).FirstOrDefault();
            if (doctor != null)
            {
                doctor.FirstName = doctorToModify.FirstName;
                doctor.LastName = doctorToModify.LastName;
                doctor.Email = doctorToModify.Email;
                try
                {
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public async Task<IEnumerable<ReceptGetter>> GetPrescriptions(int idPresciption)
        {
            var presciption = _context.Prescriptions.Where(e => e.IdPrescription == idPresciption).FirstOrDefault();
            var doctor = _context.Doctors.Where(e => e.IdDoctor == presciption.IdDoctor).FirstOrDefault();
            var patient = _context.Patients.Where(e => e.IdPatient == presciption.IdPatient).FirstOrDefault();
            var pres_med = _context.Precsription_Medicaments.Where(e => e.IdPrescription == idPresciption).FirstOrDefault();
            var medicines = _context.Medicaments.Where(e => e.IdMedicament == pres_med.IdMedicament).FirstOrDefault();
            return await _context.Prescriptions
                .Select(e => new ReceptGetter
                {
                    FirstNameClient = patient.FirstName,
                    LastNameClient = patient.LastName,
                    Birthdate = patient.Birthdate,
                    FirstNameDoctor = doctor.FirstName,
                    LastNameDoctor = doctor.LastName,
                    Medicines = e.Prescription_Medicaments.Select(e => new MedicineGetter { Name = e.Medicament.Name, Description = e.Medicament.Description, Type = e.Medicament.Type }).ToList()}).ToListAsync();
        }
    }
}
