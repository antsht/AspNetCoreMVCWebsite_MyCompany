using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyCompany.Infrastructure;

namespace MyCompany.Domain;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configuration = configurationBuilder.Build();
        
        // Get connection string from configuration
        var config = configuration.GetSection("Project").Get<AppConfig>() 
            ?? throw new InvalidOperationException("Unable to load configuration from appsettings.json");

        // Create DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(config.Database.ConnectionString)
                      .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));

        return new AppDbContext(optionsBuilder.Options);
    }
} 