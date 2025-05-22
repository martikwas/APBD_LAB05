using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

[Table("Prescription_Medicament")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }

    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }

    public int Dose { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}