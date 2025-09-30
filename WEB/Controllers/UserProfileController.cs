using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Net;
using UseCase;
using UseCase.Admin_side;
using UseCase.User_side;
using WEB.Models;

namespace WEB.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PurchasedProductController _purchasedProductController;
        private readonly UserController _userController;
        private readonly UserReviewController _userReviewController;
        public UserProfileController(UserReviewController userReviewController, PurchasedProductController purchasedProductController, UserController userController, ILogger<HomeController> logger)
        {
            _userReviewController = userReviewController;
            _purchasedProductController = purchasedProductController;
            _userController = userController;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Profile()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = int.Parse(userIdStr);
            var user = _userController.GetById(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var userView = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreatedDate = user.CreatedDate,
                Role = user.Role
            };
            
            return View(userView);
        }
        [HttpGet]
        public IActionResult EditProfile()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = int.Parse(userIdStr);
            var user = _userController.GetById(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var userView = new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role
            };

            return View(userView);
        }
        [HttpPost]
        public IActionResult EditProfile(UserViewModel userView)
        {
            var users = _userController.GetAll();
            var user = users.FirstOrDefault(u => u.Id == userView.Id);
            var mailCheck = users.FirstOrDefault(u => u.Email == userView.Email);
            if (user != null)
            {
                if (userView.Email != user.Email)
                {
                    if (mailCheck != null)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại!");
                    }
                }
                user.FullName = userView.FullName;
                user.PhoneNumber = userView.PhoneNumber;
                user.Address = userView.Address;
                user.Email = userView.Email;
                _userController.Update(user);
            }
            if (!ModelState.IsValid)
            {
                // Nếu có lỗi thì quay lại view, giữ lại dữ liệu đã nhập
                return View(userView);
            }
            TempData["Message"] = "Sửa thông tin thành công!";
            return RedirectToAction("Profile", "UserProfile");
        }
        public IActionResult PurchaseProduct()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = int.Parse(userIdStr);
            var user = _userController.GetById(userId);
            if (user == null)
            {
                TempData["Error"] = "Người dùng không tồn tại!";
                return RedirectToAction("Cart");
            }

            return View(new PurchasedProductListViewModel()
            {
                PurchasedProducts = _purchasedProductController.GetAll()
                .Where(p => p.UserId == userId)
                .Select(p => new PurchasedProductViewModel()
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    PurchasedDate = p.PurchasedDate,
                    Status = p.Status
                    
                })
            });
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePaswordViewModel userView)
        {
            var userStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userStr))
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = int.Parse(userStr);
            var user = _userController.GetById(userId);
            if (user == null)
            {
                TempData["Error"] = "Người dùng không tồn tại!";
                return RedirectToAction("Login", "Home");
            }
            if (user.Password != userView.OldPassword)
            {
                TempData["Error"] = "Mật khẩu không chính xác";
                return View(userView);
            }
            if (userView.ReEnterPassword != userView.NewPassword)
            {
                TempData["Error"] = "Mật khẩu nhập lại không chính xác";
                return View(userView);
            }
            user.Password = userView.NewPassword;
            _userController.Update(user);
            TempData["Message"] = "Thay đổi mật khẩu thành công";
            return RedirectToAction("Login", "Home");
        }

        public IActionResult CancelAccount()
        {
            var userStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userStr))
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = int.Parse(userStr);
            var user = _userController.GetById(userId);
            if (user == null)
            {
                TempData["Error"] = "Người dùng không tồn tại!";
                return RedirectToAction("Login", "Home");
            }
            user.Password = "DaXoaChuaLamBro";
            _userController.Update(user);
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Role");
            TempData["Message"] = "Hủy tài khoản thành công";
            return RedirectToAction("Login", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
