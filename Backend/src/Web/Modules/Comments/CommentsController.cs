using Microsoft.AspNetCore.Mvc;

namespace Serenity.Modules.Comments;

[Route("posts/{id}")]
public class CommentsController : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<string>> AddFriend([FromRoute] string userId, [FromRoute] string id)
    {
        return $"{userId} {id}";
    }
}