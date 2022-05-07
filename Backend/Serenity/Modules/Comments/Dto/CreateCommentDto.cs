namespace Serenity.Modules.Comments.Dto;

public class CreateCommentDto
{
    public string Content { get; set; }
    public string RepliesToId { get; set; }
}