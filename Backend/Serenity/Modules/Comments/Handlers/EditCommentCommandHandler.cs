using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Modules.Comments.Handlers;

public record EditCommentCommand(EditCommentDto Dto, ClaimsPrincipal Claims, string PostId) : IRequest<EditCommentResponse>;

public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, EditCommentResponse>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public EditCommentCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<EditCommentResponse> Handle(EditCommentCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);
        var comment = context.Comments.Where(x => x.Id == command.Dto.CommentId && x.UserId == user.Id && x.PostId == command.PostId).First();

        if (comment is null)
        {
            return new EditCommentResponse(false, new() { new("CommentNotFound", $"the Comment with the Id of {command.Dto.CommentId} does not exist") });
        }

        var updatedComment = mapper.Map<Comment>(comment);
        updatedComment.Content = command.Dto.Content;

        context.Entry(comment).CurrentValues.SetValues(updatedComment);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new EditCommentResponse(true, null);
        }

        return new EditCommentResponse(false, new() { new("EditCommentError", $"Could not edit the Comment of Id {command.Dto.CommentId}") });
    }
}