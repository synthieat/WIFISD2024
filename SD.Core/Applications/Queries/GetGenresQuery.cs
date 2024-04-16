using MediatR;
using SD.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Applications.Queries
{
    public class GetGenresQuery : IRequest<IEnumerable<Genre>>
    {

    }
}
