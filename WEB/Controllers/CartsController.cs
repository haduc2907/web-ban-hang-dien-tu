using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UseCase;
using UseCase.Admin_side;
using UseCase.User_side;
using WEB.Models;

namespace WEB.Controllers
{
    public class CartsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CartController _cartController;
        private AdminProductController _adController;
        private readonly AuthController _authController;

        public CartsController(CartController cartController, AdminProductController adController, AuthController authController, ILogger<HomeController> logger)
        {
            _cartController = cartController;
            _adController = adController;
            _authController = authController;
            _logger = logger;
        }

        
        [HttpGet]
        public IActionResult Cart()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = int.Parse(userIdStr);
            var cartItems = _cartController.GetAll(userId); // Lấy từ in-memory
            
            return View(new CartItemListViewModel()
            {
                CartItems = cartItems.Select(p => new CartItemViewModel()
                {
                    ProductId = p.ProductId,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Name = p.Name,
                    Quantity = p.Quantity
                })
            });

        }
        public IActionResult AddToCart(int productId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = int.Parse(userIdStr);
            var product = _adController.GetById(productId);
            if (product != null)
            {
                _cartController.AddToCart(product, userId);
                TempData["Message"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            }
            _logger.LogInformation("Add to productId: {id}", productId);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UpdateCart(int productId, int? increase, int? decrease, int? remove, string checkout)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = int.Parse(userIdStr);
            var carts = _cartController.GetAll(userId);
            if (increase.HasValue)
            {
                var item = carts.FirstOrDefault(c => c.ProductId == increase.Value);
                if (item != null) item.Quantity++;
            }

            if (decrease.HasValue)
            {
                var item = carts.FirstOrDefault(c => c.ProductId == decrease.Value);
                if (item != null && item.Quantity > 1) item.Quantity--;
            }

            if (remove.HasValue)
            {
                var item = carts.FirstOrDefault(c => c.ProductId == remove.Value);
                if (item != null) carts.Remove(item);
            }
            return RedirectToAction("Cart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
