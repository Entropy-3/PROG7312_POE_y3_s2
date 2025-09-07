using Microsoft.AspNetCore.Mvc;

namespace PROG7312_POE.Controllers
{
    public class IssueController : Controller
    {
        public IActionResult AddIssue()
        {
            return View();
        }
        public IActionResult ViewIssues()
        {
            return View();
        }
    }
}
