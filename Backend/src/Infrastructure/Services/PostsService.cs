using Application.Interfaces;
using Domain.Entities;

public class PostsService : IPostsService
{
    public async Task<List<Post>> GetPostsAsync(User user)
    {
        throw new NotImplementedException();
    }
}