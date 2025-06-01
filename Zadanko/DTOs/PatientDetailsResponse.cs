namespace zadanko.DTOs;

public class PatientDetailsResponse
{
    public PatientDto Patient { get; set; } = null!;
    public List<PrescriptionDetailsDto> Prescriptions { get; set; } = null!;
}