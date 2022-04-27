using Serenity.Common.Errors;

namespace Serenity.Common;

public class PaginatedResponse<T> : Response
{
    public bool Success { get; set; }
    public int Pages { get; set; }
    public int CurrentPage { get; set; }
    public T Data { get; set; }
    public List<ApplicationError> Errors { get; set; }

    public PaginatedResponse(bool success, List<ApplicationError> errors, int pages, int currentPage, T data) : base(success, errors)
    {
        Data = data;
        Pages = pages;
        CurrentPage = currentPage;
    }

    public PaginatedResponse()
    {

    }
}