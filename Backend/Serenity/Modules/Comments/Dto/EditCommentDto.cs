using Serenity.Common;
using Serenity.Common.Errors;

namespace Serenity.Modules.Comments.Dto;

public class EditCommentDto
{
    public string Content { get; set; }
    public string CommentId { get; set; }
}

public class EditCommentResponse : Response
{
    public EditCommentResponse(bool success, List<ApplicationError> errors) : base(success, errors)
    {

    }

    public EditCommentResponse() { }
}