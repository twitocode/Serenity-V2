using Application.Dtos.Posts;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPostsService
{
    Task<CreatePostResponse> CreatePostAsync(User user);
}