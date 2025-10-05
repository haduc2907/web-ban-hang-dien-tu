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
            private readonly OrderController _orderController;
            private readonly PurchasedProductController _purchasedProductController;
            private readonly AdminCategoryController _adminCategoryController;
            public AdminController(OrderController orderController, PurchasedProductController purchasedProductController, AdminCategoryController categoryController, ILogger<HomeController> logger)
            {
                _orderController = orderController;
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
        [HttpGet]
        public IActionResult Orders()
        {
            OrderListViewModel orderListViewModel = new OrderListViewModel()
            {
                Orders = _orderController.GetAllOrders().Select(List => new OrderViewModel() 
                {
                    Id = List.Id,
                    UserId = List.UserId,
                    UserName = List.UserName,
                    OrderDate = List.OrderDate,
                    TotalAmount = List.TotalAmount
                })

            };
            return View(orderListViewModel);
        }
        [HttpGet]
        public IActionResult DetailOrder(int orderId)
        {
            PurchasedProductListViewModel list = new PurchasedProductListViewModel()
            {
                PurchasedProducts = _purchasedProductController.GetAllByOrderId(orderId).Select(p => new PurchasedProductViewModel()
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    UserName = p.UserName,
                    UserId = p.UserId,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    PurchasedDate = p.PurchasedDate,
                    Status = p.Status
                })
            };
            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }

