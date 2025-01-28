using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PrescriptionsController : ControllerBase {
    private readonly PrescriptionDbContext _context;

    public PrescriptionsController(PrescriptionDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionRequest request) {
        var prescriptionId = PrescriptionIdGenerator.GeneratePrescriptionId();
        var prescription = new Prescription {
            PrescriptionId = prescriptionId,
            PatientTCID = request.PatientTCID,
            PrescriptionDetails = request.Medicines.Select(m => new PrescriptionDetails {
                MedicineName = m.MedicineName,
                Dosage = m.Dosage
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return Ok(new { PrescriptionId = prescriptionId });
    }

    [HttpGet("{prescriptionId}")]
    public async Task<IActionResult> GetPrescription(string prescriptionId) {
        if (string.IsNullOrEmpty(prescriptionId)) {
            return BadRequest("PrescriptionId cannot be null or empty.");
        }

        if (!int.TryParse(prescriptionId, out int prescriptionIdInt)) {
            return BadRequest("Invalid PrescriptionId format. PrescriptionId must be a number.");
        }

        var prescription = await _context.Prescriptions
            .Include(p => p.PrescriptionDetails)
            .FirstOrDefaultAsync(p => p.PrescriptionId == prescriptionIdInt);
        
        if (prescription == null) {
            return NotFound($"Prescrition with ID '{prescriptionId}' not found.");
        }

        var result = new {
            PrescriptionId = prescription.PrescriptionId,
            PatientTCID = prescription.PatientTCID,
            Medicines = prescription.PrescriptionDetails.Select(d => new {
                MedicineName = d.MedicineName,
                Dosage = d.Dosage
            })
        };

        return Ok(result);
    }
}

public class CreatePrescriptionRequest {
    public required string PatientTCID { get; set; }
    public required List<MedicineRequest> Medicines { get; set; }
}

public class MedicineRequest {
    public required string MedicineName { get; set; }
    public required string Dosage { get; set; }
}