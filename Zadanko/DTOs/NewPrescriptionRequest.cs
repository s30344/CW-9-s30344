namespace zadanko.DTOs;

public class NewPrescriptionRequest
{
    public PatientDto Patient { get; set; } = null!;
    public List<PrescriptionMedicamentDto> Medicaments { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDto Doctor { get; set; } = null!;
}