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
        private PurchasedProductController _purchasedProductController;
        private UserController _userController;
        private CartController _cartController;
        private AdminProductController _adController;
        private readonly AuthController _authController;

        public CartsController(PurchasedProductController purchasedProductController, UserController userController, CartController cartController, AdminProductController adController, AuthController authController, ILogger<HomeController> logger)
        {
            _purchasedProductController = purchasedProductController;
            _userController = userController;
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
                    UserId = userId,
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
            var item = _cartController.GetById(productId, userId);
            if (item == null)
            {
                if (product != null)
                {
                    _cartController.AddToCart(product, userId);
                }
            }
            else
            {
                if (product != null)
                {
                    _cartController.UpdateQuantity(productId, 1, userId);
                }
            }
            TempData["Message"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UpdateCart(int? increase, int? decrease, int? remove)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = int.Parse(userIdStr);
            if (increase.HasValue)
            {
                _cartController.UpdateQuantity(increase.Value, 1, userId);
            }

            if (decrease.HasValue)
            {
                var product = _cartController.GetById(decrease.Value, userId);
                if (product != null)
                {
                    if (product.Quantity - 1 <= 0)
                    {
                        _cartController.Delete(decrease.Value, userId);
                    }
                    else
                    {
                        _cartController.UpdateQuantity(decrease.Value, -1, userId);
                    }
                }
            }

            if (remove.HasValue)
            {
                _cartController.Delete(remove.Value, userId);

            }
            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = int.Parse(userIdStr);
            var carts = _cartController.GetAll(userId);
            if (carts.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Cart");
            }
            return View(new CartItemListViewModel() 
            {
                CartItems = carts.Select(p => new CartItemViewModel()
                {
                    ProductId = p.ProductId,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Name = p.Name,
                    Quantity = p.Quantity
                })
            });
        }
        public IActionResult ConfirmCheckout()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = int.Parse(userIdStr);
            var user = _userController.GetById(userId);
            if (user == null)
            {
                TempData["Error"] = "Người dùng không tồn tại!";
                return RedirectToAction("Cart");
            }
            var carts = _cartController.GetAll(userId);
            if (carts.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Cart");
            }
            foreach(var item in carts)
            {
                var product = _adController.GetById(item.ProductId);
                if (product != null)
                {
                    if (item.Quantity > product.Quantity)
                    {
                        TempData["Error"] = $"Số lượng sản phẩm {product.Name} trong kho không đủ!";
                        return RedirectToAction("Cart");
                    }
                    product.Quantity -= item.Quantity;
                    _adController.Update(product);

                    _purchasedProductController.Add(new PurchasedProducts()
                    {
                        UserName = user.UserName,
                        ProductId = product.Id,
                        UserId = user.Id,
                        Name = product.Name,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        PurchasedDate = DateTime.Now
                    });
                }
            }
            
            _cartController.Clear(userId);
            TempData["Message"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "Home");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
