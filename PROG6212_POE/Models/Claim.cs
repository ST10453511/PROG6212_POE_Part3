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

        // Part 3 Requirement: This is pulled from the User table automatically
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

        // ===== LOGIC YOU WANT TO KEEP =====

        /// <summary>
        /// Calculates total amount based on hours and rate.
        /// Call this before saving to the database.
        /// </summary>
        public void CalculateTotalAmount()
        {
            if (HoursWorked < 0 || HourlyRate < 0)
                throw new InvalidOperationException("Hours and Rate must be positive values.");

            TotalAmount = HoursWorked * HourlyRate;
        }
    }
}