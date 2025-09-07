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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that checks to see if the user is in the database and if the password is correct
        [HttpPost]
        public IActionResult Login(string email, string Password)
        {
            email = email.ToLower();
            //var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == Password);
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            //if statement checks to see if user is in database as well as checks if the password is correct

            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View();
            }

            //grabs the user information from the database into the session allowing it to be accessed throughout the application
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

    }
}