namespace zadanko.Controllers;
using zadanko.DTOs;
using zadanko.Exceptions;
using zadanko.Services;
using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] NewPrescriptionRequest request)
    {
        try
        {
            var id = await _prescriptionService.AddPrescriptionAsync(request);
            return Ok(new { IdPrescription = id });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}