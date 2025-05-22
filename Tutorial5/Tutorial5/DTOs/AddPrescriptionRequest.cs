namespace Tutorial5.DTOs;

public class AddPrescriptionRequest
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDto Doctor { get; set; }
    public PatientDto Patient { get; set; }
    public List<AddPrescriptionMedicamentDto> Medicaments { get; set; }
}
