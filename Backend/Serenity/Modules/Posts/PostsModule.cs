using Serenity.Common.Interfaces;

namespace Serenity.Modules.Posts;

public static class PostsModule
{
    public static IServiceCollection AddPostsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPostsService, PostsService>();
        return services;
    }
}