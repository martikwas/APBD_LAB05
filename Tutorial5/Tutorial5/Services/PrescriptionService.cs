using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;

namespace Tutorial5.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly DatabaseContext _context;

    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(AddPrescriptionRequest req)
    {
        if (req.Medicaments.Count > 10)
            throw new ArgumentException("Too many medicaments (max 10)");

        if (req.DueDate < req.Date)
            throw new ArgumentException("DueDate must be >= Date");

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p =>
                p.FirstName == req.Patient.FirstName &&
                p.LastName == req.Patient.LastName &&
                p.Birthdate == req.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = req.Patient.FirstName,
                LastName = req.Patient.LastName,
                Birthdate = req.Patient.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        foreach (var m in req.Medicaments)
        {
            if (!await _context.Medicaments.AnyAsync(x => x.IdMedicament == m.IdMedicament))
                throw new ArgumentException($"Medicament {m.IdMedicament} does not exist");
        }

        var prescription = new Prescription
        {
            Date = req.Date,
            DueDate = req.DueDate,
            IdDoctor = req.Doctor.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = req.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Description = m.Description
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientDetailsDto?> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(r => r.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(r => r.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
            return null;

        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionMedicaments
                        .Select(pm => new MedicamentDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Description = pm.Description,
                            Dose = pm.Dose
                        }).ToList()
                }).ToList()
        };
    }
}
