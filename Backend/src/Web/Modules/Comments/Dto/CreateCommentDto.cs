using Serenity.Common;
using Serenity.Common.Errors;

namespace Serenity.Modules.Comments.Dto;

public class CreateCommentDto
{
    public string Content { get; set; }
    public string RepliesToId { get; set; }
}

public class CreateCommentResponse : Response
{
    public CreateCommentResponse(bool success, List<ApplicationError> errors) : base(success, errors)
    {

    }

    public CreateCommentResponse() { }
}