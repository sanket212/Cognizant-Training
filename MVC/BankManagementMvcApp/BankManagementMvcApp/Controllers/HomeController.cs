using System.Diagnostics;
using BankManagementMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
   
namespace BankManagementMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public class ErrorViewModel 
        { 
            public string RequestId { get; set; } 
            public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); 
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
