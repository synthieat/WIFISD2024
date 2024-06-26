﻿using SD.Rescources;
using SD.Rescources.Attributes;
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
        [LocalizedDescription(nameof(BasicRes.Ratings_0))]
        Unrated = 0,
        [LocalizedDescription(nameof(BasicRes.Ratings_10))]
        Bad = 10,
        [LocalizedDescription(nameof(BasicRes.Ratings_20))]
        Medium = 20,
        [LocalizedDescription(nameof(BasicRes.Ratings_30))]
        Great = 30
    }

    public class MovieBase
    {
        [Key] /* Wäre hier jetzt überflüssig */
        public Guid Id { get; set; }


        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(BasicRes)), 
            MinLength(2, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(BasicRes))]
        [Required (ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(BasicRes))]
        [Display(Name = nameof(MovieBase.Title), ResourceType = typeof(BasicRes))]
        public string Title { get; set; }

        [Display(Name = "Genre", ResourceType = typeof(BasicRes))]
        public int GenreId { get; set; }

        [Display(Name = "MediumType", ResourceType = typeof(BasicRes))]
        public string MediumTypeCode { get; set; }

        [Display(Name = nameof(MovieBase.Price), ResourceType = typeof(BasicRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal Price { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = nameof(MovieBase.ReleaseDate), ResourceType = typeof(BasicRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = nameof(MovieBase.Rating), ResourceType = typeof(BasicRes))]
        public Ratings Rating { get; set; } = 0;
    }
}
