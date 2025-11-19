using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Data;
using PROG6212_POE.Models;
using System;

namespace PROG6212_POE.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly AppDbContext _context;

        public HRController(AppDbContext context)
        {
            _context = context;
        }

        // === LIST USERS ===
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // === CREATE USER ===
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                TempData["Success"] = "User added successfully.";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // === EDIT USER (GET) ===
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // === EDIT USER (POST) ===
        [HttpPost]
        public IActionResult Edit(User user)
        {
            // Check if the user exists in the DB
            var existingUser = _context.Users.Find(user.UserId);
            if (existingUser == null) return NotFound();

            // Update fields manually to avoid overwriting password if not changed (optional safety)
            // For this assignment, we just update the whole entity
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.HourlyRate = user.HourlyRate;
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;

            _context.SaveChanges();
            TempData["Success"] = "User details updated.";
            return RedirectToAction("Index");
        }

        // === DELETE USER (POST) ===
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["Success"] = "User deleted successfully.";
            }
            else
            {
                TempData["Error"] = "User not found.";
            }
            return RedirectToAction("Index");
        }

        // === INVOICES (Keep this from previous step) ===
        public IActionResult Invoices()
        {
            var claims = _context.Claims
                .Where(c => c.Status == "Approved")
                .ToList(); // Simple list for now
            return View(claims);
        }
    }
}