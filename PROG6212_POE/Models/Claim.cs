using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212_POE.Models
{
    public class Claim
    {
        // ===== Database Columns =====

        [Key]
        public int ClaimId { get; set; }

        // We link this to the new User table (Lecturer)
        public int LecturerId { get; set; }

        [ForeignKey("LecturerId")]
        public virtual User? Lecturer { get; set; }

        [Required]
        public DateTime DateWorked { get; set; }

        [Required]
        public decimal HoursWorked { get; set; }

        // This is pulled from the User table automatically
        public decimal HourlyRate { get; set; }

        public decimal TotalAmount { get; set; }

        [Required]
        public string Activity { get; set; } = string.Empty;

        public string Status { get; set; } = "Submitted"; // Submitted, UnderReview, Approved, Rejected

        public string? Notes { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // File storage details
        public string? DocumentPath { get; set; }
        public string? DocumentName { get; set; }

        public void CalculateTotalAmount()
        {
            // Rule 1: Basic Validity
            // Tests expect 'ArgumentException' for negative values
            if (HoursWorked < 0 || HourlyRate < 0)
                throw new ArgumentException("Hours and Rate cannot be negative.");

            // Rule 2: Business Logic Cap
            // Tests expect an error if hours > 24 (impossible in one day)
            if (HoursWorked > 24)
                throw new ArgumentException("Cannot claim more than 24 hours in a day.");

            TotalAmount = HoursWorked * HourlyRate;
        }
    }
}