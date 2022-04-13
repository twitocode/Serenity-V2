using Domain.Entities;

namespace Application.Interfaces;

public interface IPostsService
{
    Task<List<Post>> GetPostsAsync(User user);
}