using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Common.Interfaces;

public interface ICommentsService
{
    Task<List<Comment>> GetCommentsAsync(string postId);
    Task<CreateCommentResponse> CreateCommentAsync(string postId, User user, CreateCommentDto dto);
}