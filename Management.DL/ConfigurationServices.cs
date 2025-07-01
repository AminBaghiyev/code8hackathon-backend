using Management.Core.Entities;
using Management.DL.Repositories.Abstractions;
using Management.DL.Repositories.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Management.DL;

public static class ConfigurationServices
{
    public static void AddDLServices(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Room>, Repository<Room>>();
        services.AddScoped<IRepository<Reservation>, Repository<Reservation>>();
        services.AddScoped<IRepository<Service>, Repository<Service>>();
    }
}
