using Microsoft.AspNetCore.Mvc;
using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IIssuesService _issuesService;

        public IssuesController(IIssuesService issuesService)
        {
            _issuesService = issuesService;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        public IActionResult AddIssue()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        [HttpGet]
        public async Task<IActionResult> ViewIssues()
        {
            var issues = await _issuesService.GetAllIssuesAsync();
            return View(issues);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that adds a new issue to the database
        [HttpPost]
        public async Task<IActionResult> Add(issueTBL issue, IFormFile Attachment)
        {
            var userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null)
            {
                return RedirectToAction("Login", "User");
            }

            issue.UserID = userID.Value;

            if (!ModelState.IsValid)
            {
                TempData["AlertMessage"] = "Please fix the errors before submitting.";
                return View(issue);
            }

            await _issuesService.AddIssueAsync(issue, Attachment);

            return RedirectToAction("Privacy");
        }
    }
}

