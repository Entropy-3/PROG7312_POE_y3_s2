using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Services(string? status)
        {
            var all = await _serviceService.GetAllServicesAsync();
            var urgent = await _serviceService.GetTopUrgentAsync(10);

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<RequestStatus>(status, out var st))
            {
                all = all.Where(x => x.Status == st).ToList();
            }

            ViewBag.TopUrgent = urgent;
            ViewBag.UserName = HttpContext.Session.GetString("UserName") ?? "there";
            return View(all);
        }

        public async Task<IActionResult> Track(int id)
        {
            var req = await _serviceService.GetByIdAsync(id);
            if (req is null) return NotFound($"No service request with ID {id}.");

            var related = await _serviceService.GetRelatedAsync(id);
            ViewBag.Related = related;

            return View(req); // model: serviceTBL
        }

        [HttpPost]
        public async Task<IActionResult> AdvanceStatus(int id, RequestStatus next)
        {
            var ok = await _serviceService.AdvanceStatusAsync(id, next);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Track), new { id });
        }

        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddService(serviceTBL model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _serviceService.AddServiceAsync(model);
            return RedirectToAction("Services"); // or wherever you list requests
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\