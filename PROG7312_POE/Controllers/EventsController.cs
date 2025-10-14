using Microsoft.AspNetCore.Mvc;

namespace PROG7312_POE.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Events()
        {
            return View();
        }
        public IActionResult AddEvent()
        {
            return View();
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\