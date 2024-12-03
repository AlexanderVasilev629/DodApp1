using DogsApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DogsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Вземете връзката от appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Добавяне на DbContext и конфигуриране на връзка с базата данни
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Добавяне на идентичност за управление на потребителите
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Конфигуриране на Identity с индивидуални настройки за паролите
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; 
                options.Password.RequireDigit = false;           
                options.Password.RequireLowercase = false;      
                options.Password.RequireUppercase = false;      
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;             
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Добавяне на MVC
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Конфигуриране на HTTP конвейера
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Конфигуриране на пътищата
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
