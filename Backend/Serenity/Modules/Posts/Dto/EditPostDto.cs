using Serenity.Common;
using Serenity.Common.Errors;

namespace Serenity.Modules.Posts.Dto;

public class EditPostDto
{
    public string Content { get; set; }
}

public class EditPostResponse : Response
{
    public EditPostResponse(bool success, List<ApplicationError> errors) : base(success, errors)
    {

    }

    public EditPostResponse() { }
}