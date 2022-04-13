using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Handlers;

public class PostsHandler
{
    private readonly IPostsService postService;

    public PostsHandler(IPostsService postService)
    {
        this.postService = postService;
    }

    public async Task<List<Post>> GetPostsAsync(User user)
    {
        return await postService.GetPostsAsync(user);
    }
}