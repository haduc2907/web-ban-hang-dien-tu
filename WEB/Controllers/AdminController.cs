using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UseCase.Admin_side;
using WEB.Models;


namespace WEB.Controllers
    {
        public class AdminController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly AdminCategoryController _adminCategoryController;
            public AdminController(AdminCategoryController categoryController, ILogger<HomeController> logger)
            {
            _adminCategoryController = categoryController;
                _logger = logger;
            }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }

