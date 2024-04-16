using MediatR;
using SD.Core.Applications.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Applications.Queries
{
    public class GetMovieDtosQuery : IRequest<IEnumerable<MovieDto>>
    {
        public int? GenreId { get; set; }   
        public string MediumTypeCode { get; set; }

        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0;
    }
}
