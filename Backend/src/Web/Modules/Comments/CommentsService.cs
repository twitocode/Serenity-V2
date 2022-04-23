using AutoMapper;
using Serenity.Common.Interfaces;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Comments;

public class CommentsService : ICommentsService
{
    private readonly DataContext context;
    private readonly IMapper mapper;

    public CommentsService(DataContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public Task<List<Comment>> GetCommentsAsync(string postId)
    {
        var post = context.Posts.Where(post => post.Id == postId).FirstOrDefault();
        return Task.FromResult(post.Comments);
    }

    public Task<bool> Delete(User user, string postId, string commentId)
    {
        var comment = context.Comments.Where(x => x.Id == postId && x.UserId == user.Id && x.PostId == postId).FirstOrDefault();

        if (comment is null) return Task.FromResult(false);

        foreach (var s in comment.ToString()) { }

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}