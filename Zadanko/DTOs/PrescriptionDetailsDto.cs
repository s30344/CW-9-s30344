namespace zadanko.DTOs;

public class PrescriptionDetailsDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDetailsDto> Medicaments { get; set; } = null!;
    public DoctorDto Doctor { get; set; } = null!;
}