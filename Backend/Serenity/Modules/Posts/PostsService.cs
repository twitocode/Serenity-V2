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

        return new CreatePostResponse();
    }

    public Task<List<Post>> GetUserPostsAsync(User user)
    {
        var posts = context.Posts.Where(x => x.UserId == user.Id);
        return Task.FromResult(posts.ToList());
    }

    public Task<List<Post>> GetRecentPostsAsync()
    {
        var posts = context.Posts.Where(x => true).OrderByDescending(x => true);
        return Task.FromResult(posts.ToList());
    }

    public Task<List<Post>> GetUserPostsByIdAsync(string id)
    {
        var posts = context.Posts.Where(x => x.UserId == id);
        return Task.FromResult(posts.ToList());
    }

    public Task<Post> GetPostByIdAsync(string id)
    {
        var post = context.Posts.Where(x => x.UserId == id).FirstOrDefault();
        return Task.FromResult(post);
    }

    public Task<bool> DeletePostAsync(User user, string id)
    {
        var post = context.Posts.Where(x => x.Id == id && x.UserId == user.Id).FirstOrDefault();
        if (post is null) return Task.FromResult(false);

        foreach (Comment comment in post.Comments)
        {
            context.Comments.Remove(comment);
        }

        user.Posts.Remove(post);
        context.Posts.Remove(post);

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task<EditPostResponse> EditPostAsync(User user, string id, EditPostDto dto)
    {
        var post = context.Posts.Where(x => x.Id == id && x.UserId == user.Id).FirstOrDefault();

        if (post is null)
        {
            return Task.FromResult(new EditPostResponse(false, new() { new("PostNotFound", $"the post with the Id of {id} does not exist") }));
        }

        var updatedPost = mapper.Map<Post>(post);
        updatedPost.Content = dto.Content;

        context.Entry(post).CurrentValues.SetValues(updatedPost);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return Task.FromResult(new EditPostResponse(true, null));
        }

        return Task.FromResult(new EditPostResponse(false, new() { new("EditPostError", $"Could not edit the post of Id {id}") }));
    }
}