using Serenity.Common.Errors;

namespace Serenity.Common;

public class PaginatedResponse<T> : Response<T>
{
    public int Pages { get; set; }
    public int CurrentPage { get; set; }

    public PaginatedResponse(
        bool success,
        List<ApplicationError> errors,
        int pages,
        int currentPage,
        T data
    ) : base(success, errors, data)
    {
        Pages = pages;
        CurrentPage = currentPage;
    }

    public PaginatedResponse()
    {

    }
}