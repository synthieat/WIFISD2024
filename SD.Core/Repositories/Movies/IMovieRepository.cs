using SD.Core.Applications.Results;
using SD.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SD.Core.Repositories.Movies
{
    public interface IMovieRepository : IBaseRepository
    {
        IQueryable<Movie> GetMoviesWithNavigationPropertiesQuery(Expression<Func<Movie, bool>> whereFilter = null);
    }
}
