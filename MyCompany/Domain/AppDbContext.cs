using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCompany.Domain.Entities;

namespace MyCompany.Domain;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<Service> Services { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        string adminName = "admin";
        string roleAdminId = "76dfbce6-6800-413f-abed-36c2eb8e22e1";
        string userAdminId = "e9e1aeb9-2bac-4d58-a9aa-b077d160c5bf";
        string userEmail = "admin@admin.com";

        // добавляем роль администратора сайта
        builder.Entity<IdentityRole>().HasData(new IdentityRole()
        {
            Id = roleAdminId,
            Name = adminName,
            NormalizedName = adminName.ToUpper()
        });

        // добавляем нового IdentityUser для администратора сайта
        builder.Entity<IdentityUser>().HasData(new IdentityUser()
        {
            Id = userAdminId,
            UserName = adminName,
            NormalizedUserName = adminName.ToUpper(),
            Email = userEmail,
            NormalizedEmail = userEmail.ToUpper(),
            EmailConfirmed = true,
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(new IdentityUser(), adminName),
            SecurityStamp = string.Empty,
            PhoneNumber = "555-55-55",
            PhoneNumberConfirmed = true,
        });

        // добавляем роль админу
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
        {
            RoleId = roleAdminId,
            UserId = userAdminId
        });
    }
}
