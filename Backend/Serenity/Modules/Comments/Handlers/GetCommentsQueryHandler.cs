using System.Security.Claims;
using AutoMapper;
using MediatR;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Comments.Handlers;

public record GetCommentsQuery(string PostId) : IRequest<List<Comment>>;

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, List<Comment>>
{
    private readonly DataContext context;
    private readonly IMapper mapper;

    public GetCommentsQueryHandler(IMapper mapper, DataContext context)
    {
        this.mapper = mapper;
        this.context = context;
    }

    public async Task<List<Comment>> Handle(GetCommentsQuery command, CancellationToken token)
    {
        var post = context.Posts.Where(post => post.Id == command.PostId).FirstOrDefault();
        return await Task.FromResult(post.Comments);
    }
}