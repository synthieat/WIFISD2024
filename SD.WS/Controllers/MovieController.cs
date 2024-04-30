using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD.Core.Applications.Queries;
using SD.Core.Applications.Results;

namespace SD.WS.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]    
    public class MovieController : MediatRBaseController
    {
        /* Konstante für ID - Parameter */
        private const string ID_PARAMETER = "/{Id}";

        [HttpGet(nameof(MovieDto) + ID_PARAMETER)]
        public async Task<MovieDto> GetMovieDto([FromRoute] GetMovieDtoQuery query, CancellationToken cancellationToken)
        {
            return await base.Mediator.Send(query, cancellationToken);
        }


        [HttpGet(nameof(MovieDto))]
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery]GetMovieDtosQuery query, CancellationToken cancellationToken)
        {            
            return await base.Mediator.Send(query, cancellationToken);
        }
    }
}
