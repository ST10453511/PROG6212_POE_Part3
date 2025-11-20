using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Data;
using PROG6212_POE.Models;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace PROG6212_POE.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Programme Coordinator") || User.IsInRole("Academic Manager"))
            {
                return RedirectToAction("ReviewClaims", "Claims");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var claims = _context.Claims
                .Where(c => c.LecturerId == userId)
                .OrderByDescending(c => c.SubmittedAt)
                .ToList();

            return View(claims);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public IActionResult SubmitClaim()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var lecturer = _context.Users.Find(userId);

            var model = new PROG6212_POE.Models.Claim
            {
                // We added '?' to handle if lecturer is null (just in case)
                HourlyRate = lecturer?.HourlyRate ?? 0,
                DateWorked = DateTime.Today,
                HoursWorked = 0
            };

            return View(model);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(PROG6212_POE.Models.Claim model, IFormFile? document)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var lecturer = _context.Users.Find(userId);

            if (lecturer == null)
            {
                ModelState.AddModelError("", "Lecturer profile not found.");
                return View(model);
            }

            model.HourlyRate = lecturer.HourlyRate;
            model.LecturerId = userId;

            try
            {
                model.CalculateTotalAmount();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (model.HoursWorked > 24)
                ModelState.AddModelError("HoursWorked", "You cannot claim more than 24 hours in a day.");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (document != null && document.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await document.CopyToAsync(memoryStream);
                    model.FileData = memoryStream.ToArray();
                    model.DocumentName = document.FileName;
                }
            }

            _context.Claims.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Claim submitted! Total: R{model.TotalAmount:N2}";
            return RedirectToAction("MyClaims");
        }

        public IActionResult DownloadFile(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim == null || claim.FileData == null)
            {
                return NotFound("File not found in database.");
            }

            return File(claim.FileData, "application/octet-stream", claim.DocumentName ?? "document.pdf");
        }

        [HttpGet]
        public IActionResult GetClaimStatus()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var data = _context.Claims
                .Where(c => c.LecturerId == userId)
                .Select(c => new { id = c.ClaimId, status = c.Status })
                .ToList();

            return Json(data);
        }

        [Authorize(Roles = "Lecturer")]
        public IActionResult MyClaims()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var claims = _context.Claims
                .Where(c => c.LecturerId == userId)
                .OrderByDescending(c => c.SubmittedAt)
                .ToList();

            return View(claims);
        }
    }
}