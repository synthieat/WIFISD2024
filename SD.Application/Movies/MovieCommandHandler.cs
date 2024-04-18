using MediatR;
using Microsoft.EntityFrameworkCore;
using SD.Application.Base;
using SD.Core.Applications.Commands;
using SD.Core.Applications.Results;
using SD.Core.Attributes;
using SD.Core.Entities.Movies;
using SD.Core.Repositories.Movies;
using SD.Persistence.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Movies
{
    [MapServiceDependency(nameof(MovieCommandHandler))]
    public class MovieCommandHandler : BaseHandler,
                                       IRequestHandler<CreateMovieDtoCommand, MovieDto>,
                                       IRequestHandler<UpdateMovieDtoCommand, MovieDto>,
                                       IRequestHandler<DeleteMovieDtoCommand>
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
           await this.movieRepository.AddAsync<Movie>(movie, true, cancellationToken);

           /* einfach ein Default Objekt zurück geben mit einer vom System generierten ID */
            return await Task.FromResult(MovieDto.MapFrom(movie));            
        }
                
        public async Task<MovieDto> Handle(UpdateMovieDtoCommand command, CancellationToken cancellationToken)
        {
            command.MovieDto.Id = command.Id;

            //var movie = await this.movieRepository.QueryFrom<Movie>(w => w.Id == command.Id).FirstOrDefaultAsync(cancellationToken);

            //if (movie == null)
            //{
            //    movie = new Movie { Id = command.Id };
            //    await this.movieRepository.AddAsync(movie, false, cancellationToken);
            //}

            var movie = new Movie();

            base.MapEntityProperties(command.MovieDto, movie);

            var updMovie = await this.movieRepository.UpdateAsync<Movie>(movie, command.Id, false, cancellationToken);
            await this.movieRepository.SaveAsync(cancellationToken);

            return MovieDto.MapFrom(updMovie);
        }


        public async Task Handle(DeleteMovieDtoCommand command, CancellationToken cancellationToken)
        {
            await this.movieRepository.RemoveByKeyAsync<Movie>(command.Id, true, cancellationToken);
        }
    }
}
