using Management.Core.Entities;
using Management.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace Management.DL.Contexts;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Service> Services { get; set; }

    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region Roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "0081da26-93e7-4af0-8f90-108ca3378634", Name = Role.Customer.ToString(), NormalizedName = Role.Customer.ToString().ToUpper() },
            new IdentityRole { Id = "5f5276ed-8bf3-46cc-9c1d-8d427cb907d9", Name = Role.Admin.ToString(), NormalizedName = Role.Admin.ToString().ToUpper() }
        );
        #endregion

        #region Admin
        AppUser admin = new()
        {
            Id = "767b9ec5-6a12-4ee8-9169-dfa0ef38eaab",
            FullName = "Admin",
            Email = "baghiyev.amin@gmail.com",
            PhoneNumber = "+994708143593",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
        };

        PasswordHasher<AppUser> hasher = new();
        admin.PasswordHash = hasher.HashPassword(admin, "admin123!");
        builder.Entity<AppUser>().HasData(admin);

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = admin.Id, RoleId = "5f5276ed-8bf3-46cc-9c1d-8d427cb907d9" }
        );
        #endregion

        base.OnModelCreating(builder);
    }
}
