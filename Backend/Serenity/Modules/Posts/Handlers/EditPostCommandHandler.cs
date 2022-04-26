using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts.Handlers;

public record EditPostCommand(EditPostDto Dto, ClaimsPrincipal Claims, string Id) : IRequest<EditPostResponse>;

public class EditPostCommandHandler : IRequestHandler<EditPostCommand, EditPostResponse>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public EditPostCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<EditPostResponse> Handle(EditPostCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);
        var post = context.Posts.Where(x => x.Id == command.Id && x.UserId == user.Id).First();

        if (post is null)
        {
            return new EditPostResponse(false, new() { new("PostNotFound", $"the post with the Id of {command.Id} does not exist") });
        }

        var updatedPost = mapper.Map<Post>(post);
        updatedPost.Content = command.Dto.Content;

        context.Entry(post).CurrentValues.SetValues(updatedPost);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new EditPostResponse(true, null);
        }

        return new EditPostResponse(false, new() { new("EditPostError", $"Could not edit the post of Id {command.Id}") });
    }
}