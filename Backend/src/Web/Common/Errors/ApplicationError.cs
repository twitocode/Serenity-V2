namespace Serenity.Common.Errors;

public class ApplicationError
{
    public string Code { get; set; }
    public string Description { get; set; }

    public ApplicationError(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public ApplicationError() { }
}