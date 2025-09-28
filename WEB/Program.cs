using Infractructure;
using Infractructure.SQL;
using System.Data.SqlTypes;
using UseCase;
using UseCase.Admin_side;
using UseCase.User_side;

namespace WEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<AdminProductController>();
            builder.Services.AddTransient<AuthController>();
            builder.Services.AddTransient<CartController>();
            builder.Services.AddTransient<UserController>();
            builder.Services.AddTransient<AdminCategoryController>();
            builder.Services.AddTransient<UserProductController>();
            builder.Services.AddTransient<PurchasedProductController>();
            builder.Services.AddTransient<UserReviewController>();
            if (builder.Configuration["DataSource"] == "InMemory")
            {
                builder.Services.AddSingleton<IAdminProductControllerRepository, InMemoryProductRepository>();
                builder.Services.AddSingleton<ICartControllerRepository, InMemoryCartRepository>();
                builder.Services.AddSingleton<IAuthRepository, InMemoryAuthRepository>();
                builder.Services.AddSingleton<IUserControllerRepository, InMemoryUserRepository>();
                builder.Services.AddSingleton<IAdminCategoryControllerRepository, InMemoryCategoryRepository>();
                builder.Services.AddSingleton<IUserProductControllerRepository, InMemoryProductRepository>();
                builder.Services.AddSingleton<IPurchasedProductControllerRepository, InMemoryPurchasedProductRepostiory>();
                builder.Services.AddSingleton<IUserReviewControllerRepository, InMemoryUserReviewRepository>();
            }
            else
            {
                builder.Services.AddSingleton<IAdminProductControllerRepository, SqlProductRepository>();
                builder.Services.AddSingleton<ICartControllerRepository, SqlCartRepository>();
                builder.Services.AddSingleton<IAuthRepository, SqlAuthRepository>();
                builder.Services.AddSingleton<IUserControllerRepository, SqlUserRepository>();
                builder.Services.AddSingleton<IAdminCategoryControllerRepository, SqlCategoryRepository>();
                builder.Services.AddSingleton<IUserProductControllerRepository, SqlProductRepository>();
                builder.Services.AddSingleton<IPurchasedProductControllerRepository, SqlPurChasedProductRepository>();
                builder.Services.AddSingleton<IUserReviewControllerRepository, SqlUserReviewRepository>();
            }

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // th?i gian h?t h?n
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
