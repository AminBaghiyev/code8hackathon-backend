using FluentValidation;
using FluentValidation.AspNetCore;
using Management.BL.Services.Abstractions;
using Management.BL.Services.Implementations;
using Management.BL.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Management.DL;

public static class ConfigurationServices
{
    public static void AddBLServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddScoped<EmailService>();
        services.AddSingleton<JWTService>();

        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDashboardService, DashboardService>();
    }
}
