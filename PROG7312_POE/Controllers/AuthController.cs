using Microsoft.AspNetCore.Mvc;
using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        // Constructor that injects the authentication service
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        public IActionResult Login()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that registers a new user
        [HttpPost]
        public async Task<IActionResult> SignUp(userTBL user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var (success, errorMessage) = await _authService.RegisterUserAsync(user);

            if (!success)
            {
                ModelState.AddModelError("Email", errorMessage);
                return View(user);
            }

            return RedirectToAction("Login");
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that logs the user in by creating session information
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _authService.AuthenticateUserAsync(email, password);

            if (user == null)
            {
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View();
            }

            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that logs the user out by removing session information
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\