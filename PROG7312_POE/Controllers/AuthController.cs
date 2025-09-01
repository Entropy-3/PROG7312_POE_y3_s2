using Microsoft.AspNetCore.Mvc;

namespace PROG7312_POE.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
    }
}
