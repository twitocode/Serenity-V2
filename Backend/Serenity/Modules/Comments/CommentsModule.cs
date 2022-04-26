using Serenity.Common.Interfaces;

namespace Serenity.Modules.Comments;

public static class CommentsModule
{
    public static IServiceCollection AddCommentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}