using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class PrescriptionDbContext : DbContext {
    public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options) : base(options) { }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionDetails> PrescriptionDetails { get; set; }
}

public static class PrescriptionIdGenerator {
    private static Random _random = new Random();
    public static int GeneratePrescriptionId() {
        return _random.Next(100000000, 999999999);
    }
}

public class Prescription {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int PrescriptionId { get; set; }
    [Required]
    public required string PatientTCID { get; set; }
    public required ICollection<PrescriptionDetails> PrescriptionDetails { get; set; }
}

public class PrescriptionDetails {
    [Key]
    public int Id { get; set; }
    [Required]
    public int PrescriptionId { get; set; }
    [ForeignKey("PrescriptionId")]
    public Prescription? Prescription { get; set; }
    [Required]
    public required string MedicineName { get; set; }
    [Required]
    public required string Dosage { get; set; }
}