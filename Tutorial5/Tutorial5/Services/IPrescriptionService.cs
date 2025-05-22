using Tutorial5.DTOs;

namespace Tutorial5.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(AddPrescriptionRequest request);
    Task<PatientDetailsDto?> GetPatientDetailsAsync(int idPatient);

}


