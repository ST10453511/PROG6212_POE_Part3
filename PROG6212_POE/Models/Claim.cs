using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212_POE.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        public int LecturerId { get; set; }

        [ForeignKey("LecturerId")]
        public virtual User? Lecturer { get; set; }

        [Required]
        public DateTime DateWorked { get; set; }

        [Required]
        public decimal HoursWorked { get; set; }

        public decimal HourlyRate { get; set; }

        public decimal TotalAmount { get; set; }

        [Required]
        public string Activity { get; set; } = string.Empty;

        public string Status { get; set; } = "Submitted"; 

        public string? Notes { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public byte[]? FileData { get; set; } 
        
        public string? DocumentName { get; set; }

        public void CalculateTotalAmount()
        {
            if (HoursWorked < 0 || HourlyRate < 0)
                throw new ArgumentException("Hours and Rate cannot be negative.");

            if (HoursWorked > 24)
                throw new ArgumentException("Cannot claim more than 24 hours in a day.");

            TotalAmount = HoursWorked * HourlyRate;
        }
    }
}