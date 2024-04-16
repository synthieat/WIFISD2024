using MediatR;
using SD.Core.Applications.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Applications.Commands
{
    public class CreateMovieDtoCommand : IRequest<MovieDto>
    {
    }
}
