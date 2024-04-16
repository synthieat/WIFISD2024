using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Entities.Movies
{
    public enum Ratings: byte
    {
        Unrated = 0,
        Bad = 10,
        Medium = 20,
        Great = 30
    }

    public class MovieBase
    {
        [Key] /* Wäre hier jetzt überflüssig */
        public Guid Id { get; set; }
        [MaxLength(128), MinLength(2)]
        [Required]
        public string Title { get; set; }

        public int GenreId { get; set; }

        public string MediumTypeCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal Price { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

        public Ratings Rating { get; set; } = 0;
    }
}
