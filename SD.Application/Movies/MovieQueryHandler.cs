﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using SD.Core.Applications.Queries;
using SD.Core.Applications.Results;
using SD.Core.Attributes;
using SD.Core.Entities.Movies;
using SD.Core.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Movies
{
    [MapServiceDependency(nameof(MovieQueryHandler))]
    public class MovieQueryHandler : IRequestHandler<GetMovieDtoQuery, MovieDto>,
                                     IRequestHandler<GetMovieDtosQuery, IEnumerable<MovieDto>>,
                                     IRequestHandler<GetGenresQuery, IEnumerable<Genre>>,
                                     IRequestHandler<GetMediumTypesQuery, IEnumerable<MediumType>>
    {
        protected readonly IMovieRepository movieRepository;

        public MovieQueryHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository; 
        }

        public async Task<MovieDto> Handle(GetMovieDtoQuery request, CancellationToken cancellationToken)
        {

            return await this.movieRepository.QueryFrom<Movie>(w => w.Id == request.Id).Select(s => MovieDto.MapFrom(s)).FirstOrDefaultAsync(cancellationToken);

            /* Old fashion code
            var movie = await this.movieRepository.QueryFrom<Movie>(w => w.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (movie == null)
            {
                return MovieDto.MapFrom(movie);
            }
            else
            {
                return null;
            }*/
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetMovieDtosQuery request, CancellationToken cancellationToken)
        {
            return await this.movieRepository.QueryFrom<Movie>().Select(s => MovieDto.MapFrom(s)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Genre>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            return await this.movieRepository.QueryFrom<Genre>().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MediumType>> Handle(GetMediumTypesQuery request, CancellationToken cancellationToken)
        {
            return await this.movieRepository.QueryFrom<MediumType>().ToListAsync(cancellationToken);
        }
    }
}
