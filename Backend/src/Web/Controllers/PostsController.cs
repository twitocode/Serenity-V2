using Microsoft.AspNetCore.Mvc;

[Route("posts")]
public class PostsController : ControllerBase
{
    [HttpGet]
    public void GetPosts()
    {

    }
}