using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Serenity.Common;

public class ApiControllerBase : ControllerBase
{
    private IMediator _mediator = null!;
    protected IMediator mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}