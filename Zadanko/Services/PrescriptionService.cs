namespace zadanko.Services;
using zadanko.Models;
using zadanko.DTOs;
using zadanko.Data;
using zadanko.Exceptions;
using Microsoft.EntityFrameworkCore;

public class PrescriptionService : IPrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PatientDetailsResponse> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
        {
            throw new NotFoundException($"Pacjent z ID {idPatient} nie został znaleziony");
        }

        var prescriptions = patient.Prescriptions
            .OrderBy(p => p.DueDate)
            .Select(p => new PrescriptionDetailsDto
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
                Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDetailsDto
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Details
                }).ToList()
            }).ToList();

        return new PatientDetailsResponse
        {
            Patient = new PatientDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.BirthDate
            },
            Prescriptions = prescriptions
        };
    }

    public async Task<int> AddPrescriptionAsync(NewPrescriptionRequest request)
    {
        if (request.Medicaments.Count > 10)
        {
            throw new BadRequestException("Recepta może posiadać max. 10 leków");
        }

        if (request.DueDate < request.Date)
        {
            throw new BadRequestException("Termin musi być równy lub późniejszy od daty");
        }
        
        var medicamentIds = request.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicamentsCount = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .CountAsync();

        if (existingMedicamentsCount != request.Medicaments.Count)
        {
            throw new NotFoundException("Nie znaleziono żadnych leków");
        }
        
        var patient = await _context.Patients.FindAsync(request.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                IdPatient = request.Patient.IdPatient,
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                BirthDate = request.Patient.BirthDate
            };
            _context.Patients.Add(patient);
        }
        
        var doctor = await _context.Doctors.FindAsync(request.Doctor.IdDoctor);
        if (doctor == null)
        {
            doctor = new Doctor
            {
                IdDoctor = request.Doctor.IdDoctor,
                FirstName = request.Doctor.FirstName,
                LastName = request.Doctor.LastName,
                Email = request.Doctor.Email
            };
            _context.Doctors.Add(doctor);
        }
        
        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = doctor.IdDoctor,
            PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return prescription.IdPrescription;
    }
}