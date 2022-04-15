using Microsoft.AspNetCore.Mvc;

namespace Serenity.Modules.Friends;

[Route("friends")]
public class FriendsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<bool>> AddFriend()
    {
        return true;
    }
}