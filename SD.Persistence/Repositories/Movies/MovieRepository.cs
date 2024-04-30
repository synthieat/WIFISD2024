using Microsoft.EntityFrameworkCore;
using SD.Core.Attributes;
using SD.Core.Entities.Movies;
using SD.Core.Repositories.Movies;
using SD.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SD.Persistence.Repositories.Movies
{
    [MapServiceDependency(nameof(MovieRepository))]
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public IQueryable<Movie> GetMoviesWithNavigationPropertiesQuery(Expression<Func<Movie, bool>> whereFilter = null)
        {
            var query =  base.QueryFrom<Movie>().Include(i => i.Genre).Include(i => i.MediumType);
            if (whereFilter != null)
            {
                return query.Where(whereFilter);
            }

            return query;               
             
        }
    }
}
