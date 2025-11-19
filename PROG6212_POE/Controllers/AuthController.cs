using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Data;
using PROG6212_POE.Models;
using System;
using System.Security.Claims;
using AuthClaim = System.Security.Claims.Claim;

namespace PROG6212_POE.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        // Inject the Database Context instead of IUserStore
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl)
        {
            // 1. Query the Database using LINQ
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewData["Error"] = "Invalid username or password.";
                return View();
            }

            // 2. Create User Claims
            var claims = new List<AuthClaim>
            {
                new AuthClaim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // ID is now an int
                new AuthClaim(ClaimTypes.Name, user.FullName),
                new AuthClaim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // REDIRECTION BASED ON ROLE
            if (user.Role == "HR")
            {
                return RedirectToAction("Index", "HR");
            }
            else if (user.Role == "ProgrammeCoordinator" || user.Role == "AcademicManager")
            {
                // Managers go to the Review page, NOT the Dashboard
                return RedirectToAction("Index", "Management");
            }
            else
            {
                // Lecturers go to the Dashboard
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Denied() => View();
    }
}