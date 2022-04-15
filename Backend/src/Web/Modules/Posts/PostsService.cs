using AutoMapper;
using Serenity.Common.Interfaces;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts;

public class PostsService : IPostsService
{
    private readonly DataContext context;
    private readonly IMapper mapper;

    public PostsService(DataContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<CreatePostResponse> CreatePostAsync(User user, CreatePostDto dto)
    {
        Post post = mapper.Map<Post>(dto);
        post.User = user;

        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();

        return new CreatePostResponse
        {
            Errors = null,
            Success = true,
        };
    }

    public Task<List<Post>> GetUserPostsAsync(User user)
    {
        var posts = context.Posts.Where(x => x.UserId == user.Id);
        return Task.FromResult(posts.ToList());
    }

    public Task<bool> DeletePostAsync(User user, string id)
    {
        var post = context.Posts.Where(x => x.Id == id && x.UserId == user.Id).FirstOrDefault();
        if (post is null) return Task.FromResult(false);

        context.Posts.Remove(post);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}