using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Common.Interfaces;

public interface ICommentsService
{
    Task<List<Comment>> GetCommentsAsync(string postId);
    Task<CreateCommentResponse> CreateCommentAsync(string postId, User user, CreateCommentDto dto);
    Task<CreateCommentResponse> ReplyToCommentAsync(string postId, User user, CreateCommentDto dto);
    Task<EditCommentResponse> EditCommentAsync(string postId, User user, EditCommentDto dto);
    Task<bool> DeleteAsync(string postId, User user, string commentId);
}