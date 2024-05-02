using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.AccessControl;

namespace SD.WS.Controllers
{
    public class MediatRBaseController : ControllerBase
    {
        private IMediator mediator;

        protected IMediator Mediator => this.mediator ??= HttpContext.RequestServices.GetService<IMediator>();
     
        
        protected T SetLocationUri<T>(T result, string id)
        {
            if(result == null || string.IsNullOrWhiteSpace(id))
            {
                throw new HttpRequestException("Resource is null!");
            }

            /* Aktueller URL ermitteln */
            var baseUrl = Request.HttpContext.Request.GetEncodedUrl();

            /* Base URL bis zum ersten (QueryString) Parameter, falls vorhanden, kürzen */
            var length = baseUrl.IndexOf('?') > 0 ? baseUrl.IndexOf('?') : baseUrl.Length;
            var uri = baseUrl.Substring(0, length);

            /* Id an den gekürzten URL anhängen => URI der neuen Ressource */
            /*.../Movie/MovieDto
            * .../Movie/MovieDto/
            * .../Movie/MovieDto?Parameter1 = ...
            * .../Movie/MovieDto/?Parameter1 = ... */

            uri = string.Concat(uri, uri.EndsWith('/') ? string.Empty : "/", id);

            /* Location header hinzufügen */
            HttpContext.Response.Headers.Append("Location", uri);

            /* Http-Status Code 201 - Created setzen */
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
           
            return result;
        }
    }
}
