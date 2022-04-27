using AutoMapper;
using MediatR;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Comments.Handlers;

public record GetCommentsQuery(string PostId, int Page) : IRequest<PaginatedResponse<List<Comment>>>;

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, PaginatedResponse<List<Comment>>>
{
    private readonly DataContext context;
    private readonly IMapper mapper;

    public GetCommentsQueryHandler(IMapper mapper, DataContext context)
    {
        this.mapper = mapper;
        this.context = context;
    }

    public async Task<PaginatedResponse<List<Comment>>> Handle(GetCommentsQuery command, CancellationToken token)
    {
        float commentsPerPage = 10f;
        double pageCount = Math.Ceiling(context.Comments.Where(x => x.PostId == command.PostId).Count() / commentsPerPage);

        var comments = context.Comments
            .Where(x => x.PostId == command.PostId)
            .Skip((command.Page - 1) * (int)commentsPerPage)
            .Take((int)commentsPerPage)
            .ToList();

        return await Task.FromResult(new PaginatedResponse<List<Comment>>
        {
            CurrentPage = command.Page,
            Data = comments,
            Errors = null,
            Pages = (int)pageCount
        });
    }
}