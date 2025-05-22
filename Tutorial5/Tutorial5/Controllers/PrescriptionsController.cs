using Microsoft.AspNetCore.Mvc;
using Tutorial5.Services;
using Tutorial5.DTOs; 

namespace Tutorial5.Controllers
{
    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;

        public PrescriptionsController(IPrescriptionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionRequest request)
        {
            try
            {
                await _service.AddPrescriptionAsync(request);
                return Ok("Prescription added.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}