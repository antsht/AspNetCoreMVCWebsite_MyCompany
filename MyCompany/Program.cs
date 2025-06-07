using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCompany.Domain;
using MyCompany.Infrastructure;

namespace MyCompany;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // подключаем в конфигурацию файл appsettings.json
        IConfigurationBuilder configBuild = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        // оборачиваем секцию Project в форму объекта для удобства
        IConfiguration configuration = configBuild.Build();
        AppConfig config = configuration.GetSection("Project").Get<AppConfig>() 
            ?? throw new FileNotFoundException("Can't load config from appsettings.json"); 

        // подключаем контекст базы данных
        builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString));

        // настраиваем Identity систему
        builder.Services.AddIdentity<IdentityUser,IdentityRole>(
            options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        // настраиваем Auth Cookies
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "myCompanyAuth";
            options.Cookie.HttpOnly = true;
            options.LoginPath = "/admin/login";
            options.AccessDeniedPath = "/admin/accessdenied";
            options.SlidingExpiration = true;
        });

        // подключаем функционал контроллеров
        builder.Services.AddControllersWithViews();

        // Собираем конфигурацию
        WebApplication app = builder.Build();

        // ! Порядок следования middleware очень важен !

        // Подключаем использование статических файлов
        app.UseStaticFiles();

        // подключаем систему маршрутизации
        app.UseRouting();

        // подключаем аутентификацию и авторизацию
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        // регистрируем маршрут
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}
