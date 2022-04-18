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
}