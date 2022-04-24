using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Common.Interfaces;

public interface IPostsService
{
    Task<CreatePostResponse> CreatePostAsync(User user, CreatePostDto dto);
    Task<EditPostResponse> EditPostAsync(User user, string id, EditPostDto dto);
    Task<List<Post>> GetUserPostsAsync(User user);
    Task<List<Post>> GetRecentPostsAsync();
    Task<List<Post>> GetUserPostsByIdAsync(string id);
    Task<Post> GetPostByIdAsync(string id);
    Task<bool> DeletePostAsync(User user, string id);
}