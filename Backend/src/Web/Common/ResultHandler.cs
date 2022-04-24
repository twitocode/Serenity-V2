using Microsoft.AspNetCore.Mvc;

namespace Serenity.Common;

public static class ResultHandler
{
    public static IActionResult Handle(Response response)
    {
        if (response.Success) return new OkObjectResult(response);
        return new BadRequestObjectResult(response);
    }

    public static IActionResult Handle(bool response)
    {
        if (response) return new OkObjectResult(response);
        return new BadRequestObjectResult(response);
    }
}