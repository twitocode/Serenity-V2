using System.Security.Claims;
using Application.Dtos.Auth;
using Application.Dtos.Posts;
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

    public async Task<CreatePostResponse> CreatePostAsync(User user)
    {
        return await postService.CreatePostAsync(user);
    }
}