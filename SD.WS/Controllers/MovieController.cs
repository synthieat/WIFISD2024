using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD.Core.Applications.Commands;
using SD.Core.Applications.Queries;
using SD.Core.Applications.Results;
using System.Net;

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
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery] GetMovieDtosQuery query, CancellationToken cancellationToken)
        {
            return await base.Mediator.Send(query, cancellationToken);
        }

        [ProducesResponseType(typeof(MovieDto), (int)HttpStatusCode.Created)]
        [HttpPost(nameof(MovieDto))]
        public async Task<MovieDto> CreateMovieDto(CancellationToken cancellationToken)
        {
            var command = new CreateMovieDtoCommand();
            var result = await base.Mediator.Send(command, cancellationToken);

            return base.SetLocationUri<MovieDto>(result, result.Id.ToString());
        }


        [HttpPut(nameof(MovieDto) + ID_PARAMETER)]
        public async Task<MovieDto> UpdateMovieDto([FromRoute] Guid Id, [FromBody] MovieDto movieDto, CancellationToken cancellationToken)
        {
            var command = new UpdateMovieDtoCommand { Id = Id, MovieDto = movieDto };
            return await base.Mediator.Send(command, cancellationToken);
        }

        /* Alternative, wenn die Attribute [FromRoute]/[Frombody] im Command definiert werden
        public async Task<MovieDto> UpdateMovieDto([FromQuery] UpdateMovieDtoCommand command, CancellationToken cancellationToken)
        {

            return await base.Mediator.Send(command, cancellationToken);
        }*/


        [HttpDelete(nameof(MovieDto) + ID_PARAMETER)]
        public async Task DeleteMovieDto([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var command = new DeleteMovieDtoCommand { Id = Id };
            await base.Mediator.Send(command, cancellationToken);
        }
    }
}
