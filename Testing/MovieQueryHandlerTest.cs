using Moq;
using NUnit.Framework;

using SD.Application.Movies;
using SD.Core.Applications.Queries;
using SD.Core.Applications.Results;
using SD.Core.Entities.Movies;
using SD.Core.Repositories.Movies;
using System.Linq.Expressions;

namespace Testing
{
    public class MovieQueryHandlerTest : BaseServiceTests<MovieQueryHandler>
    {
       

        [Test]
        public async Task GetAllMovies()
        {
            var request = new GetMovieDtosQuery();
            var movies = await Service.Handle(request, CancellationToken);

            Assert.That(movies.Count, Is.GreaterThan(0));

        }


    }
}
