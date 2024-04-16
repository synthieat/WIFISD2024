using MediatR;
using SD.Core.Applications.Commands;
using SD.Core.Applications.Results;
using SD.Core.Entities.Movies;
using SD.Core.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Movies
{
    public class MovieCommandHandler : IRequestHandler<CreateMovieDtoCommand, MovieDto>
    {

        private IMovieRepository movieRepository;

        public MovieCommandHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<MovieDto> Handle(CreateMovieDtoCommand command, CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "n/a",
                GenreId = 1,
                MediumTypeCode = "BR"
            };

            /* Hier wir die Ressource sofort in die DB gespeichert, aber unvollständig */
            //await this.movieRepository.AddAsync<Movie>(movie, true, cancellationToken);

           /* einfach ein Default Objekt zurück geben mit einer vom System generierten ID */
            return await Task.FromResult(MovieDto.MapFrom(movie));            
        }
    }
}
