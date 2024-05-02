using SD.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Applications.Results
{
    public class MovieDto: MovieBase
    { 
        public string? GenreName { get; set; }
        public string? MediumTypeName { get; set; }
        public string? LocalizedRating { get; set; }


        public static MovieDto MapFrom(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                MediumTypeCode = movie.MediumTypeCode,
                MediumTypeName = movie.MediumType?.Name??string.Empty,
                GenreId = movie.GenreId,
                GenreName = movie.Genre?.Name??string.Empty,
                Price = movie.Price,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating
            };
        }
    }
}
