using Microsoft.EntityFrameworkCore;
using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.Services.Interfaces;
using PRN222.Milktea.Service.Services;
using PRN222.Milktea.RazorPage.Hubs;
using PRN222.Milktea.Service.Mappers;

namespace PRN222.Milktea.RazorPage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddDbContext<MilkteaSaleDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddSession();
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            var app = builder.Build();

           

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Configure middleware
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapHub<OrderHub>("/orderHub");
            app.Run();
        }
    }
}
