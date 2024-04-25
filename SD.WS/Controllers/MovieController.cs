using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD.Core.Applications.Queries;
using SD.Core.Applications.Results;

namespace SD.WS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class MovieController : ControllerBase
    {

        [HttpGet(nameof(MovieDto))]
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery]GetMovieDtosQuery query, CancellationToken cancellationToken)
        {
            var mediatR = HttpContext.RequestServices.GetService<IMediator>();
            return await mediatR.Send(query, cancellationToken);
        }
    }
}
