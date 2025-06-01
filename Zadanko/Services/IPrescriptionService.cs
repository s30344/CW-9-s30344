namespace zadanko.Services;
using zadanko.DTOs;
public interface IPrescriptionService
{
    Task<PatientDetailsResponse> GetPatientDetailsAsync(int idPatient);
    Task<int> AddPrescriptionAsync(NewPrescriptionRequest request);
}