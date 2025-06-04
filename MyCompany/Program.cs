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

        // подключаем функционал контроллеров
        builder.Services.AddControllersWithViews();

        // Собираем конфигурацию
        WebApplication app = builder.Build();

        // ! Порядок следования middleware очень важен !

        // Подключаем использование статических файлов
        app.UseStaticFiles();

        // подключаем систему маршрутизации
        app.UseRouting();

        // регистрируем маршрут
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}
