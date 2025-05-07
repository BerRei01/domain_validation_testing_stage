using Application.Common.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Composers;

public static class RegisterDependencies
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMonitoringBehavior<,>));        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMonitoringBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}