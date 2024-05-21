using Microsoft.AspNetCore.Mvc.Rendering;
using SD.Core.Applications.Results;

namespace SD.Web.Models.Movies
{
    public class MovieModel
    {
        public MovieDto Movie { get; set; }
        public SelectList Genre { get; set; }
        public SelectList MediumType { get; set; }
        public SelectList Ratings { get; set; }       


    }
}
