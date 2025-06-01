namespace zadanko.Controllers;
using zadanko.Exceptions;
using zadanko.Services;
using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PatientsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet("{idPatient}")]
    public async Task<IActionResult> GetPatientDetails(int idPatient)
    {
        try
        {
            var result = await _prescriptionService.GetPatientDetailsAsync(idPatient);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
