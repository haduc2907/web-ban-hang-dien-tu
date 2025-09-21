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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AdminCategoryController _adminCategoryController;
        private UserController _userController;
        private CartController _cartController;
        private AdminProductController _adController;
        private readonly AuthController _authController;

        public HomeController(AdminCategoryController adminCategoryController, UserController userController, CartController cartController, AdminProductController adController, AuthController authController, ILogger<HomeController> logger)
        {
            _adminCategoryController = adminCategoryController;
            _userController = userController;
            _cartController = cartController;
            _adController = adController;
            _authController = authController;
            _logger = logger;
        }

        public IActionResult Index(int? categoryId)
        {
            
            var categories = _adminCategoryController.GetAll();
            IEnumerable<Product> products;
            if (categoryId.HasValue)
            {
                products = _adController.GetByCategoryId(categoryId.Value);
            }
            else
            {
                products = _adController.GetAll(); // mặc định lấy hết
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

            var user = new User()
            {
                UserName = userM.UserName,
                Email = userM.Email,
                Password = userM.Password
            };

            _authController.Register(user);
            _userController.Add(user);

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
            if (!ModelState.IsValid)
            {
                return View(p);
            }
            var status = p.Quantity == 0
        ? EStatusProduct.OutOfStock
        : EStatusProduct.Available;
            _adController.Add(new Entities.Product()
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
                    Brand = p.Brand


                });
            }
            return RedirectToAction("Index");
            
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
