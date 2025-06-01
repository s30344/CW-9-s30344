namespace zadanko.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("Prescription_Medicament")]
public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    public virtual Medicament Medicament { get; set; } = null!;
    
    public int IdPrescription { get; set; }
    public virtual Prescription Prescription { get; set; } = null!;
    
    public int? Dose { get; set; }
    
    [MaxLength(100)]
    public string Details { get; set; } = null!;
    
    [Key]
    public int IdPrescriptionMedicament { get; set; }
}