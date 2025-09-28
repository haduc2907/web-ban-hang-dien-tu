using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using UseCase;
using UseCase.Admin_side;
using UseCase.User_side;
using WEB.Models;

namespace WEB.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdminCategoryController _adminCategoryController;
        public CategoriesController(AdminCategoryController categoryController, ILogger<HomeController> logger)
        {
            _adminCategoryController = categoryController;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(CategoryViewModel category)
        {
            _adminCategoryController.Add(new Categories()
            {
                Name = category.Name
            });
            TempData["Message"] = "Thêm danh mục thành công!";
            return RedirectToAction("Manager");

        }

        public IActionResult Manager()
        {
            var categories = _adminCategoryController.GetAll();
            return View(new ProductListViewModel()
            {
                Products = [],
                Categories = categories.Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
            });
        }
        public IActionResult Delete(int categoryId)
        {
            try
            {
                var categories = _adminCategoryController.GetAll();
                var category = _adminCategoryController.GetById(categoryId);
                if (category != null)
                {
                    _adminCategoryController.Delete(categoryId);
                    TempData["Message"] = "Xóa danh mục thành công!";
                }

                return RedirectToAction("Manager");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa danh mục");
                TempData["Error"] = "Không thể xóa danh mục này vì có sản phẩm liên quan.";
                return RedirectToAction("Manager");
            }


        }
        [HttpGet]
        public IActionResult Update(int categoryId)
        {
            var category = _adminCategoryController.GetById(categoryId);
            if (category != null)
            {
                return View(new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return RedirectToAction("Manager");

        }
        [HttpPost]
        public IActionResult Update(CategoryViewModel categoryView)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryView);
            }
            var cartegories = _adminCategoryController.GetAll();
            var cartegory = cartegories.FirstOrDefault(c => c.Id == categoryView.Id);
            if (cartegory != null)
            {
                cartegory.Name = categoryView.Name;
                _adminCategoryController.Update(cartegory);
            }
            return RedirectToAction("Manager");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
