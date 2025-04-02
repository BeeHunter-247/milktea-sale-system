using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.Repositories;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.Mappers;
using PRN222.Milktea.Service.Services;
using PRN222.Milktea.Service.Services.Interfaces;

namespace PRN222.Milktea.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Set up logging (useful for debugging)
            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });

            // Configure DbContext
            builder.Services.AddDbContext<MilkteaSaleDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register services and repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IComboService, ComboService>();

            // AutoMapper configuration
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Add controllers and views
            builder.Services.AddControllersWithViews();

            // Set up Authentication (Cookie Authentication)
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Index";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                });

            // Optionally, if session is needed, you can add session support
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);  // Set session timeout
                options.Cookie.HttpOnly = true;
            });

            // Build the application
            var app = builder.Build();

            // Configure middleware for HTTP requests
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Use routing and authentication
            app.UseRouting();

            app.UseAuthentication();  // Add this to ensure authentication middleware is applied
            app.UseAuthorization();   // Add this to ensure authorization middleware is applied

            // Add session middleware if required
            app.UseSession();

            // Set up the default controller route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}/{id?}");

            // Run the application
            app.Run();
        }
    }
}
