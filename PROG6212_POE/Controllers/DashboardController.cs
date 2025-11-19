using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Needed for database access
using PROG6212_POE.Data;
using PROG6212_POE.Models;
using System;
using System.Security.Claims;

namespace PROG6212_POE.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileStorage _fileStorage;

        public DashboardController(AppDbContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        // === DASHBOARD (List of Claims) ===
        public IActionResult Index()
        {
            // Get the logged-in user's ID (which we stored as an Int in AuthController)
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Fetch claims from SQL Server
            var claims = _context.Claims
                .Where(c => c.LecturerId == userId)
                .OrderByDescending(c => c.SubmittedAt)
                .ToList();

            return View(claims); // Reuse the "Index" view (formerly MyClaims)
        }

        // === SUBMIT CLAIM (GET) ===
        [HttpGet]
        public IActionResult SubmitClaim()
        {
            // 1. Get the logged-in lecturer's ID
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 2. Fetch their profile to get the HourlyRate (Automation!)
            var lecturer = _context.Users.Find(userId);

            // 3. Pre-fill the form with their Rate
            var model = new PROG6212_POE.Models.Claim
            {
                HourlyRate = lecturer?.HourlyRate ?? 0, // Pulls 500 from the DB
                DateWorked = DateTime.Today,
                HoursWorked = 0
            };

            // 4. Send this non-null model to the View
            return View(model);
        }

        // === SUBMIT CLAIM (POST) ===
        [HttpPost]
        // clearly tell it to use YOUR Claim model, not the Security one
        public async Task<IActionResult> SubmitClaim(PROG6212_POE.Models.Claim model, IFormFile? document)
        {
            // 1. Get Current User
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var lecturer = _context.Users.Find(userId);

            // 2. AUTOMATION: Pull the Hourly Rate from the Database (Requirement)
            // The lecturer cannot edit this; we ignore whatever they sent in the form for 'Rate'
            model.HourlyRate = lecturer.HourlyRate;
            model.LecturerId = userId;

            // 3. AUTOMATION: Calculate Total
            try
            {
                model.CalculateTotalAmount();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            // 4. VALIDATION: Business Rules
            if (model.HoursWorked > 24)
                ModelState.AddModelError("HoursWorked", "You cannot claim more than 24 hours in a day.");

            if (model.HoursWorked > 100) // Simple monthly cap example
                ModelState.AddModelError("HoursWorked", "This exceeds the monthly limit of 100 hours.");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 5. Handle File Upload
            if (document != null)
            {
                try
                {
                    var fileData = await _fileStorage.SaveAsync(document);
                    model.DocumentPath = fileData.RelativePath;
                    model.DocumentName = fileData.FileName;
                }
                catch (Exception ex)
                {
                    // If file upload fails, show error and stop
                    TempData["Error"] = $"File upload failed: {ex.Message}";
                    return View(model);
                }
            }

            // 6. Save to SQL Database
            _context.Claims.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Claim submitted! Total: R{model.TotalAmount:N2}";
            return RedirectToAction("MyClaims");
        }

        // === TRACKING API (For the Progress Bar) ===
        [HttpGet]
        public IActionResult GetClaimStatus()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var data = _context.Claims
                .Where(c => c.LecturerId == userId)
                .Select(c => new { id = c.ClaimId, status = c.Status })
                .ToList();

            return Json(data);
        }

        // === MY CLAIMS (The List View) ===
        public IActionResult MyClaims()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var claims = _context.Claims
                .Where(c => c.LecturerId == userId)
                .OrderByDescending(c => c.SubmittedAt)
                .ToList();

            return View(claims);
        }
    }
}