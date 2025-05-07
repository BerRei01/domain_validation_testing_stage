using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common.Composer;

public static class RegisterDependencies
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IInMemoryMeetingRepository, InMemoryMeetingRepository>();
        return services;
    }
}