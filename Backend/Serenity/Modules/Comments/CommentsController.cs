using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Modules.Comments.Dto;
using Serenity.Modules.Comments.Handlers;

namespace Serenity.Modules.Comments;

[Route("posts/{postId}")]
[Authorize]
public class CommentsController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetComments([FromRoute] string postId, [FromQuery] int page)
        => ResultHandler.Handle(await mediator.Send(new GetCommentsQuery(postId, page)));

    [HttpPost]
    public async Task<IActionResult> CreateOrReply([FromRoute] string postId, [FromBody] CreateCommentDto dto)
        => ResultHandler.Handle(await mediator.Send(new CreateCommentCommand(dto, HttpContext.User, postId)));

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> Delete([FromRoute] string postId, [FromRoute] string commentId)
        => ResultHandler.Handle(await mediator.Send(new DeleteCommentCommand(commentId, HttpContext.User, postId)));

    [HttpPut("edit")]
    public async Task<IActionResult> Edit([FromRoute] string postId, [FromBody] EditCommentDto dto)
        => ResultHandler.Handle(await mediator.Send(new EditCommentCommand(dto, HttpContext.User, postId)));
}