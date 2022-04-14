using Application.Dtos.Posts;
using Application.Interfaces;
using Domain.Entities;

public class PostsService : IPostsService
{
    public async Task<CreatePostResponse> CreatePostAsync(User user)
    {
        throw new NotImplementedException();
    }
}