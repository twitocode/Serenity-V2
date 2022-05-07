using Serenity.Common.Errors;

namespace Serenity.Common;

public class Response<T>
{
    public bool Success { get; set; }
    public List<ApplicationError> Errors { get; set; }
    public T Data { get; set; }

    public Response(bool success, List<ApplicationError> errors, T data)
    {
        Success = success;
        Errors = errors;
        Data = data;
    }

    public Response()
    {

    }
}