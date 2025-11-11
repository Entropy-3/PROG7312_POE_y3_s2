using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using PROG7312_POE.Migrations;
using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that returns all service requests, with optional status filtering
        public async Task<IActionResult> Services(string? status)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var all = await _serviceService.GetAllServicesAsync();
            all = all.Where(s => s.UserID == userId.Value).ToList();

            var urgent = await _serviceService.GetTopUrgentAsync(10);
            urgent = urgent.Where(s => s.UserID == userId.Value).ToList();

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<RequestStatus>(status, out var st))
            {
                all = all.Where(x => x.Status == st).ToList();
            }

            ViewBag.TopUrgent = urgent;
            ViewBag.UserName = HttpContext.Session.GetString("UserName") ?? "there";
            return View(all);
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that returns the tracking view for a specific service request
        public async Task<IActionResult> Track(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var req = await _serviceService.GetByIdAsync(id);
            if (req is null) return NotFound($"No service request with ID {id}.");

            var related = await _serviceService.GetRelatedAsync(id);
            related = related.Where(r => r.UserID == userId.Value).ToList();
            ViewBag.Related = related;

            return View(req);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that advances the status of a service request
        [HttpPost]
        public async Task<IActionResult> AdvanceStatus(int id, RequestStatus next)
        {
            var ok = await _serviceService.AdvanceStatusAsync(id, next);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Track), new { id });
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that returns the add service view
        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that adds a new service request to the database
        [HttpPost]
        public async Task<IActionResult> AddService(serviceTBL model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {

                return RedirectToAction("Login", "Auth");
            }

            model.UserID = userId.Value;   

            await _serviceService.AddServiceAsync(model);
            return RedirectToAction("Services");
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        [HttpGet]
        public async Task<IActionResult> Admin(string? status)
        {
            string role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (role != "admin")
            {
                return Forbid();
            }

            List<serviceTBL> all = await _serviceService.GetAllServicesAsync();

            if (!string.IsNullOrWhiteSpace(status))
            {
                bool parsed = Enum.TryParse<RequestStatus>(status, out var st);
                if (parsed)
                {
                    all = all.Where(s => s.Status == st).ToList();
                }
            }

            ViewBag.SelectedStatus = status;
            return View(all);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        [HttpPost]
        public async Task<IActionResult> AdminAdvanceStatus(int id, RequestStatus next, string? statusFilter)
        {
            string role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (role != "admin")
            {
                return Forbid();
            }

            bool ok = await _serviceService.AdvanceStatusAsync(id, next);
            if (!ok)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(statusFilter))
            {
                return RedirectToAction(nameof(Admin));
            }
            else
            {
                return RedirectToAction(nameof(Admin), new { status = statusFilter });
            }
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\