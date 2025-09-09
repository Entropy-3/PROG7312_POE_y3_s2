using Microsoft.AspNetCore.Mvc;
using PROG7312_POE.Models;

namespace PROG7312_POE.Controllers
{
    public class IssuesController : Controller
    {
        private readonly AppDbContext _context;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        public IssuesController(AppDbContext context)
        {
            _context = context;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        public IActionResult AddIssue()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        public IActionResult ViewIssues()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        [HttpPost]
        public IActionResult Add(issueTBL issue, IFormFile Attachment)
        {
            var userID = HttpContext.Session.GetInt32("UserID");

            // Redirect user to login if UID is null
            if (userID == null)
            {
                return RedirectToAction("Login", "User");
            }

            issue.UserID = userID.Value;

            // Handle single attachment
            if (Attachment != null && Attachment.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Attachment.CopyTo(memoryStream);
                    issue.DocumentData = memoryStream.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                _context.Issues.Add(issue);
                _context.SaveChanges();

                return RedirectToAction("Privacy");
            }
            else
            {
                TempData["AlertMessage"] = "Please fix the errors before submitting.";

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(issue);
            }
        }
    }
}
