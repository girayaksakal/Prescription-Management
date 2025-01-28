using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly MedicineService _medicineService;

    public MedicinesController(MedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    // Refreshes the medicine list
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshMedicineList()
    {
        await _medicineService.RefreshMedicineDataAsync();
        return Ok(new { message = "Medicine list refreshed successfully." });
    }

    // Autocomplete endpoint for searching medicines
    [HttpGet("autocomplete")]
    public IActionResult Autocomplete([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return BadRequest(new { message = "Search term cannot be empty." });
        }

        var results = _medicineService.SearchMedicines(term);
        return Ok(new { medicationNames = results });
    }
}