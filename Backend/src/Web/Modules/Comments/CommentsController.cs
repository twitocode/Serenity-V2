using Microsoft.AspNetCore.Mvc;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;

namespace Serenity.Modules.Comments;

[Route("posts/{postId}")]
public class CommentsController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly ICommentsService commentsService;

    public CommentsController(IAuthService authService, ICommentsService commentsService)
    {
        this.authService = authService;
        this.commentsService = commentsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetComments([FromRoute] string postId)
    {
        var comments = await commentsService.GetCommentsAsync(postId);
        return Ok(comments);
    }
}