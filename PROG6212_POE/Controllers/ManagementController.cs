using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Data;
using PROG6212_POE.Models;
using System;

namespace PROG6212_POE.Controllers
{
    [Authorize(Roles = "ProgrammeCoordinator,AcademicManager")]
    public class ManagementController : Controller
    {
        private readonly AppDbContext _context;

        public ManagementController(AppDbContext context)
        {
            _context = context;
        }

        // === PAGE 1: The Welcome Dashboard ===
        public IActionResult Index()
        {
            return View();
        }

        // GET: List Pending Claims
        public async Task<IActionResult> ReviewClaims()
        {
            // Fetch claims that are Submitted OR UnderReview
            var claims = await _context.Claims
                .Include(c => c.Lecturer) // Join with User table to get names
                .Where(c => c.Status == "Submitted" || c.Status == "UnderReview")
                .OrderBy(c => c.SubmittedAt)
                .ToListAsync();

            return View(claims);
        }

        // POST: Start Review (Moves status from 'Submitted' to 'UnderReview')
        [HttpPost]
        public async Task<IActionResult> StartReview(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                TempData["Error"] = "Claim not found.";
                return RedirectToAction("ReviewClaims");
            }

            // Only start review if it is currently Submitted
            if (claim.Status == "Submitted")
            {
                claim.Status = "UnderReview";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Claim is now Under Review.";
            }

            return RedirectToAction("ReviewClaims");
        }

        // POST: Approve (Finalizes claim)
        [HttpPost]
        public async Task<IActionResult> Approve(int id, string? comment)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = "Approved";

            // Append comment if provided
            if (!string.IsNullOrWhiteSpace(comment))
            {
                claim.Notes += $" [Approver Comment: {comment}]";
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Claim Approved.";
            return RedirectToAction("ReviewClaims");
        }

        // POST: Reject (Finalizes claim)
        [HttpPost]
        public async Task<IActionResult> Reject(int id, string? comment)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = "Rejected";

            // Append comment if provided
            if (!string.IsNullOrWhiteSpace(comment))
            {
                claim.Notes += $" [Rejection Reason: {comment}]";
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Claim Rejected.";
            return RedirectToAction("ReviewClaims");
        }
    }
}