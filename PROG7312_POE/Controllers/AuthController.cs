using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7312_POE.Models;

namespace PROG7312_POE.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public IActionResult Login()
        {
            return View();
        }
        
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that creates user in the database
        [HttpPost]
        public IActionResult SignUp(userTBL user)
        {

            if (ModelState.IsValid)
            {
                user.Email = user.Email.ToLower();
                //checks to see if the email is already taken
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return View(user);
                }
                //hashes password for security
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            //else statement for debugging purposes
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(user);
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that logs the user out by removing session information
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        
    }
}