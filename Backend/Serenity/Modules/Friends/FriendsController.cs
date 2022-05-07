using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serenity.Common;
using Serenity.Modules.Friends.Handlers;

namespace Serenity.Modules.Friends;

//OTHER USERS FRIENDS SHOULD BE HIDDEN

[Route("friends")]
[Authorize]
public class FriendsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserFriends()
        => ResultHandler.Handle(await mediator.Send(new GetUserFriendsQuery(HttpContext.User)));

    [HttpPost("{id}")]
    public async Task<IActionResult> AddFriend([FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new AddFriendCommand(HttpContext.User, id)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFriend([FromRoute] string id)
        => ResultHandler.Handle(await mediator.Send(new RemoveFriendCommand(HttpContext.User, id)));
}

