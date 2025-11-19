using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Models;

// Bring in the security claims namespace (for ClaimTypes)
using System.Security.Claims;

// Alias the BCL Claim so it never conflicts with PROG6212_POE_P1.Models.Claim
using AuthClaim = System.Security.Claims.Claim;

namespace PROG6212_POE.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserStore _userStore;
        public AuthController(IUserStore userStore) => _userStore = userStore;

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
            => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _userStore.Validate(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            // Use the aliased type 'AuthClaim' so we don't collide with Models.Claim
            var claims = new List<AuthClaim>
            {
                new AuthClaim(ClaimTypes.NameIdentifier, user.UserId),
                new AuthClaim(ClaimTypes.Name, user.FullName),
                new AuthClaim(ClaimTypes.GivenName, user.Username),
                new AuthClaim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = model.RememberMe }
            );

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Denied() => Content("Access denied.");
    }
}



