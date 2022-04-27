using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Modules.Comments.Handlers;

public record DeleteCommentCommand(string CommentId, ClaimsPrincipal Claims, string PostId) : IRequest<Response>;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Response>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public DeleteCommentCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response> Handle(DeleteCommentCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);
        var comment = context.Comments.Where(x => x.Id == command.CommentId && x.UserId == user.Id && x.PostId == command.PostId).First();

        if (comment is null)
            return new()
            {
                Success = false,
                Errors = new() { new("CommentNotFound", $"Could not find the comment of Id {command.CommentId}") }
            };

        foreach (var reply in comment.Replies)
            context.Comments.Remove(reply);

        context.Comments.Remove(comment);
        var result = context.SaveChanges();

        if (result >= 0)
            return new()
            {
                Success = true,
                Errors = null
            };

        return new()
        {
            Success = false,
            Errors = new() { new("DeleteCommentErorr", $"Could not delete the comment of Id {command.CommentId}") }
        };
    }
}