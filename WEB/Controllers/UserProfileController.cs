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
        private readonly UserController _userController;
        public UserProfileController(UserController userController, ILogger<HomeController> logger)
        {
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
            if (!ModelState.IsValid)
            {
                // Nếu có lỗi thì quay lại view, giữ lại dữ liệu đã nhập
                return View(userView);
            }
            var users = _userController.GetAll();
            var user = users.FirstOrDefault(u => u.Id == userView.Id);
            if (user != null)
            {
                user.Email = userView.Email;
                user.PhoneNumber = userView.PhoneNumber;
                user.FullName = userView.FullName;
                user.Address = userView.Address;
                user.Role = userView.Role;
            }
            TempData["Message"] = "Sửa thông tin thành công!";
            return RedirectToAction("Profile", "UserProfile");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
