using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // Storing plain for academic demo, usually hashed

        [Required]
        public string Role { get; set; } = string.Empty; // Lecturer, ProgrammeCoordinator, AcademicManager, HR

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Automating the rate: HR sets this, Lecturer just uses it.
        public decimal HourlyRate { get; set; } = 0m;
    }
}