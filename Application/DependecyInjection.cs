using Mediator.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(typeof(DependecyInjection).Assembly);
        return services;
    }
}