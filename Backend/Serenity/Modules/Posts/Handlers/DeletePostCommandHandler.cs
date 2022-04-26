using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts.Handlers;

public record DeletePostCommand(string Id, ClaimsPrincipal Claims) : IRequest<bool>;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public DeletePostCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<bool> Handle(DeletePostCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);
        var post = context.Posts.Where(x => x.Id == command.Id && x.UserId == user.Id).First();

        if (post is null) return true;

        foreach (Comment comment in post.Comments)
        {
            context.Comments.Remove(comment);
        }

        user.Posts.Remove(post);
        context.Posts.Remove(post);

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return true;
        }

        return false;
    }
}