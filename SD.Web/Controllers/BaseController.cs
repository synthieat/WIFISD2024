using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SD.Web.Controllers
{
    public class BaseController : Controller
    {
        private IMediator mediator;
        protected IMediator Mediator => this.mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
