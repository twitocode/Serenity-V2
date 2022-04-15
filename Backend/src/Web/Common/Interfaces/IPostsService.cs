using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Common.Interfaces;

public interface IPostsService
{
    Task<CreatePostResponse> CreatePostAsync(User user, CreatePostDto dto);
    Task<List<Post>> GetUserPostsAsync(User user);
    Task<bool> DeletePostAsync(User user, string id);
}