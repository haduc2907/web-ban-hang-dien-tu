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
            private readonly PurchasedProductController _purchasedProductController;
            private readonly AdminCategoryController _adminCategoryController;
            public AdminController(PurchasedProductController purchasedProductController, AdminCategoryController categoryController, ILogger<HomeController> logger)
            {
                _purchasedProductController = purchasedProductController;
                _adminCategoryController = categoryController;
                _logger = logger;
            }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ConfirmOrder()
        {
            var purchaseProducts = _purchasedProductController.GetAll();
            return View(new PurchasedProductListViewModel()
            {
                PurchasedProducts = purchaseProducts.Select(p => new PurchasedProductViewModel() 
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    ProductId = p.ProductId,
                    UserName = p.UserName,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    PurchasedDate = p.PurchasedDate,
                    Status = p.Status
                })
            });
        }
        public IActionResult UpdateStatus(int id, EOrderStatus status)
        {
            var product = _purchasedProductController.GetById(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            product.Status = status;
            _purchasedProductController.Update(product);
            return RedirectToAction("ConfirmOrder");



        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }

