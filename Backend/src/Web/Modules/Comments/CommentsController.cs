using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Modules.Comments;

[Route("posts/{postId}")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IIdentityService authService;
    private readonly ICommentsService commentsService;

    public CommentsController(IIdentityService authService, ICommentsService commentsService)
    {
        this.authService = authService;
        this.commentsService = commentsService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetComments([FromRoute] string postId)
    {
        var comments = await commentsService.GetCommentsAsync(postId);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] string postId, [FromBody] CreateCommentDto dto)
    {
        var user = await authService.GetUserAsync(HttpContext.User);
        var result = await commentsService.CreateCommentAsync(postId, user, dto);
        
        return ResultHandler.Handle(result);
    }

    [HttpPost("{commentId}")]
    public async Task<IActionResult> Reply([FromRoute] string postId, [FromRoute] string commentId)
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] string postId, [FromRoute] string commentId)
    {
        //needs to delete comments and replies
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromRoute] string postId, [FromRoute] string commentId)
    {
        return Ok();
    }
}