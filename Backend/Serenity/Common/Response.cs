using Serenity.Common.Errors;

namespace Serenity.Common;

public class Response
{
    public bool Success { get; set; }
    public List<ApplicationError> Errors { get; set; }

    public Response(bool success, List<ApplicationError> errors)
    {
        Success = success;
        Errors = errors;
    }

    public Response()
    {

    }
}