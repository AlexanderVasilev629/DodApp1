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

            // ������� �������� �� appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // �������� �� DbContext � ������������� �� ������ � ������ �����
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // �������� �� ����������� �� ���������� �� �������������
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // ������������� �� Identity � ������������ ��������� �� ��������
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

            // �������� �� MVC
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ������������� �� HTTP ���������
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

            // ������������� �� ��������
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
