using Serenity.Database.Entities;

namespace Serenity.Common.Interfaces;

public interface ICommentsService
{
    Task<List<Comment>> GetCommentsAsync(string postId);
}