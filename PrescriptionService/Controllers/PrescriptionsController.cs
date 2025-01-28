using Microsoft.AspNetCore.Mvc;

namespace PrescriptionService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PrescriptionsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreatePrescription([FromBody] PrescriptionModel prescription)
    {
        // Simulate saving a prescription
        return CreatedAtAction(nameof(GetPrescription), new { id = 1 }, prescription);
    }

    [HttpGet("{id}")]
    public IActionResult GetPrescription(int id)
    {
        return Ok(new { Id = id, Medicine = "Paracetamol", Patient = "John Doe" });
    }
}

public class PrescriptionModel
{
    public required string PatientTCID { get; set; }
    public required string PatientName { get; set; }
    public required List<MedicinesModel> MedicineNames { get; set; }
}

public class MedicinesModel
{
    public required string MedicineNames { get; set; }
    public required string MedicineNotes { get; set; }
}