using Serenity.Common.Interfaces;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts;

public class PostsService : IPostsService
{
    private readonly DataContext context;

    public PostsService(DataContext context)
    {
        this.context = context;
    }

    public Task<CreatePostResponse> CreatePostAsync(User user, CreatePostDto dto)
    {
        var post = new Post
        {
            Comments = new List<Comment>(),
            Content = dto.Content,
            Title = dto.Title,
            Tags = dto.Tags.ToList(),
            User = user,
            UserId = user.Id
        };

        user.Posts.Add(post);
        context.SaveChanges();

        return Task.FromResult(new CreatePostResponse
        {
            Errors = null,
            Success = true,
        });
    }
}