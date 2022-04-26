using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts.Handlers;

public record CreatePostCommand(CreatePostDto Dto, ClaimsPrincipal Claims) : IRequest<CreatePostResponse>;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostResponse>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public CreatePostCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<CreatePostResponse> Handle(CreatePostCommand command, CancellationToken token)
    {
        Post post = mapper.Map<Post>(command.Dto);
        post.User = await userManager.GetUserAsync(command.Claims);

        await context.Posts.AddAsync(post);
        var result = await context.SaveChangesAsync();

        if (result >= 0)
        {
            return new(true, null);
        }

        return new(false, new() { new("CreatePostError", "Could not create the Post") });
    }
}