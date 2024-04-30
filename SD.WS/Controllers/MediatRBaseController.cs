using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace SD.WS.Controllers
{
    public class MediatRBaseController : ControllerBase
    {
        private IMediator mediator;

        protected IMediator Mediator => this.mediator ??= HttpContext.RequestServices.GetService<IMediator>();
      
    }
}
