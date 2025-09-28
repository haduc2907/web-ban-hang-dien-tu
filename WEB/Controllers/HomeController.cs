using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UseCase;
using UseCase.Admin_side;
using UseCase.User_side;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserReviewController _userReviewController;
        private readonly PurchasedProductController _purchasedProductController;
        private readonly UserProductController _userProductController;
        private readonly AdminCategoryController _adminCategoryController;
        private readonly UserController _userController;
        private readonly CartController _cartController;
        private readonly AdminProductController _adController;
        private readonly AuthController _authController;

        public HomeController(UserReviewController userReviewController, PurchasedProductController purchasedProductController, UserProductController userProductController, AdminCategoryController adminCategoryController, UserController userController, CartController cartController, AdminProductController adController, AuthController authController, ILogger<HomeController> logger)
        {
            _userReviewController = userReviewController;
            _purchasedProductController = purchasedProductController;
            _userProductController = userProductController;
            _adminCategoryController = adminCategoryController;
            _userController = userController;
            _cartController = cartController;
            _adController = adController;
            _authController = authController;
            _logger = logger;
        }
        public IActionResult Index(int? categoryId, ProductFilterOptions options)
        {
            
            var categories = _adminCategoryController.GetAll();
            IEnumerable<Products> products;
            if (categoryId.HasValue)
            {
                products = _adController.GetByCategoryId(categoryId.Value);
            }
            else
            {
                products = _adController.GetAll();
            }
            if (options != null)
            {
                options.CategoryId = categoryId;
                products = _userProductController.Filters(products, options);
            }
            return View(new ProductListViewModel()
            {
                Products = products.Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Status = p.Quantity == 0 ? EStatusProduct.OutOfStock : EStatusProduct.Available,
                    Brand = p.Brand
                }),
                Categories = categories.Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
            }
            );
        }
        public IActionResult Manager()
        {
            var products = _adController.GetAll();
            var categories = _adminCategoryController.GetAll();
            return View(new ProductListViewModel() 
            {
                Products = products.Select(p => new ProductViewModel() 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Status = p.Quantity == 0 ? EStatusProduct.OutOfStock : EStatusProduct.Available,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate,
                    CategoryId = p.CategoryId,
                    Brand = p.Brand
                 }),
                Categories = categories.Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
            });
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authController.Login(model.UserName, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Role", user.Role.ToString());
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }

                // Sai tài khoản hoặc mật khẩu
                ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu!");
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel userM)
        {
            var users = _userController.GetAll();
            var userCheck = users.FirstOrDefault(u => u.UserName == userM.UserName);

            if (userCheck != null)
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại!");
            }

            if (!ModelState.IsValid)
            {
                // Nếu có lỗi thì quay lại view, giữ lại dữ liệu đã nhập
                return View(userM);
            }

            var user = new Users()
            {
                UserName = userM.UserName,
                Email = userM.Email,
                Password = userM.Password,
                Role = ESRoleUser.Customer
            };

            _authController.Register(user);

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(ProductViewModel p)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(p);
                }
                var status = p.Quantity == 0
            ? EStatusProduct.OutOfStock
            : EStatusProduct.Available;
                _adController.Add(new Entities.Products()
                {
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Status = status,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CategoryId = p.CategoryId,
                    Brand = p.Brand

                });
                return RedirectToAction("Manager");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm");
                TempData["Error"] = "Lỗi khi thêm sản phẩm";
                return View(p);
            }

        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var p = _adController.GetById(id);
            if (p != null)
            {
                return View(new ProductViewModel()
                {
                    Id = id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Status = p.Status,
                    UpdatedDate = DateTime.Now,
                    CategoryId = p.CategoryId,
                    Brand = p.Brand


                });
            }
            return RedirectToAction("Manager");

        }
        [HttpPost]
        public IActionResult Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var products = _adController.GetAll();
            var product = products.FirstOrDefault(p => p.Id == model.Id);
            if (product != null)
            {
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                product.ImageUrl = model.ImageUrl;
                product.Status = model.Status;
                product.UpdatedDate = DateTime.Now;
                product.CategoryId = model.CategoryId;
                product.Brand = model.Brand;
                _adController.Update(product);
            }
            return RedirectToAction("Manager");

        }
        public IActionResult Delete(int Id)
        {
            _adController.Delete(Id);
            return RedirectToAction("Manager");
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var p = _adController.GetById(id);
            var r = _userReviewController.GetAll().Where(r => r.ProductId == id).ToList();
            if (p != null)
            {
                return View(new ProductViewModel()
                {
                    Id = id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Status = p.Status,
                    Brand = p.Brand,
                    Reviews = r.Select(rv => new ReviewViewModel()
                    {
                        Id = rv.Id,
                        ProductId = rv.ProductId,
                        UserId = rv.UserId,
                        UserName = rv.UserName,
                        Comment = rv.Comment,
                        Rating = rv.Rating,
                        CreatedDate = rv.CreatedDate
                    }).ToList()


                });
            }
            return RedirectToAction("Index"); 
        }
        [HttpGet]
        public IActionResult Review(int id)
        {
            _logger.LogInformation("Id: {i}", id);
            var pp = _purchasedProductController.GetById(id);
            if (pp == null)
            {
                return RedirectToAction("Index");
            }
            var p = _adController.GetById(pp.ProductId);
            if (p == null)
            {
                return RedirectToAction("Index");
            }
            return View(new PurchasedProductViewModel()
            {
                Id = id,
                Name = pp.Name,
                ProductId = pp.ProductId,
                Quantity = pp.Quantity,
                ImageUrl = pp.ImageUrl,
                Price = p.Price,
            });
        }
        [HttpPost]
        public IActionResult Review(string? comment, int rating, int productId, int id)
        {
            _logger.LogInformation("ProductId: {p}, Id: {id}", productId, id);
            var product = _adController.GetById(productId);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
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
            if (rating < 1 || rating > 5)
            {
                ModelState.AddModelError(string.Empty, "Đánh giá không hợp lệ. Vui lòng chọn từ 1 đến 5 sao.");
                return RedirectToAction("Detail", new { id = productId });
            }
            var review = new Reviews
            {
                ProductId = productId,
                UserId = userId,
                UserName = user.UserName,
                Comment = comment,
                Rating = rating,
                CreatedDate = DateTime.Now
            };
            _userReviewController.Add(review);
            _logger.LogInformation("{id}", id);
            var purchasedProduct = _purchasedProductController.GetById(id);
            if (purchasedProduct == null)
            {
                return RedirectToAction("Index");
            }
            purchasedProduct.Status = EOrderStatus.Reviewed;
            _logger.LogInformation("status: {p}", purchasedProduct.Status.ToString());
            _purchasedProductController.Update(purchasedProduct);
            return RedirectToAction("Detail", new { id = productId });


        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
