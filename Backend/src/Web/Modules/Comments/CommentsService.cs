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
}