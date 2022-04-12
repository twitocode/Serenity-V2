using Application.Handlers;

namespace Web;

public static class WebInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthHandler>();

        return services;
    }
}