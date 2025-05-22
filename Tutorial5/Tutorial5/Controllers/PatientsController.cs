using Microsoft.AspNetCore.Mvc;
using Tutorial5.Services;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PatientsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        var result = await _service.GetPatientDetailsAsync(id);
        if (result == null)
            return NotFound($"Patient with id {id} not found");

        return Ok(result);
    }
}