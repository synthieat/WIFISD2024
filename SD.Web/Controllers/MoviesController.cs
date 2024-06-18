using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GridMvc.Server;
using GridShared;
using GridShared.Sorting;
using GridShared.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD.Common.Extensions;
using SD.Core.Applications.Commands;
using SD.Core.Applications.Queries;
using SD.Core.Applications.Results;
using SD.Core.Entities.Movies;
using SD.Persistence.Repositories.DBContext;
using SD.Rescources;
using SD.Web.Components;
using SD.Web.Extensions;
using SD.Web.Models.Movies;

namespace SD.Web.Controllers
{
    //[Authorize]
    public class MoviesController : BaseController
    {
        public MoviesController(){}

        // GET: Movies
        public async Task<IActionResult> Index([FromQuery] string? SearchCriteria, CancellationToken cancellationToken)
        {            
            var result = await base.Mediator.Send(new GetMovieDtosQuery { SearchCriteria = SearchCriteria }, cancellationToken);

            ViewBag.SearchCriteria = SearchCriteria;    
                        
            return View(result);
        }


        public async Task<IActionResult>IndexGrid(string gridState = "", CancellationToken cancellationToken = default)
        {
            string returnUrl = "/Movies/IndexGrid";
            IQueryCollection query = HttpContext.Request.Query;

            if (!string.IsNullOrWhiteSpace(gridState))
            {
                try
                {
                    query = new QueryCollection(StringExtensions.GetQuery(gridState));
                }
                catch (Exception)
                {
                    // do nothing, gridState was not a valid state
                }
            }

            ViewBag.ActiveMenuTitle = "GridMVC .NETCore";
            var localeId = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            Action<IGridColumnCollection<MovieDto>> columns = c =>
            {
                /* Adding not mapped column, that render body, using inline Razor html helper */
                //c.Add()
                //    .Encoded(false)
                //    .Sanitized(false)
                //    .SetWidth(60)
                //    .Css("hidden-xs")
                //    .RenderComponentAs<ButtonCellViewComponent>(returnUrl);

                /* Adding "Id" column: */
                c.Add(o => o.Id)
                 .Titled("Id")
                 .SetWidth(150);

                /* Adding "ReleaseDate" column: */
                c.Add(o => o.ReleaseDate)
                    .Titled(BasicRes.ReleaseDate)
                    .SortInitialDirection(GridSortDirection.Descending)
                    .ThenSortByDescending(o => o.Title)
                    .SetCellCssClassesContraint(o => o.ReleaseDate >= DateTime.Parse("1997-01-01") ? "red" : "")
                    .Format("{0:yyyy-MM-dd}")
                    .SetWidth(110)
                    .Max(true).Min(true);

                /* Adding "Title" column: */
                c.Add(o => o.Title)
                    .Titled(BasicRes.Title)
                    .ThenSortByDescending(o => o.GenreName)
                    .ThenSortByDescending(o => o.MediumTypeName)
                    .SetWidth(250)
                    .Max(true).Min(true);
            };

            var movieDtos = (await this.Mediator.Send(new GetMovieDtosQuery(), cancellationToken));
            var gridServer = new GridServer<MovieDto>(movieDtos, query, false, "moviesGrid", columns, 10, localeId)
                .Sortable()
                .Filterable()
                .WithMultipleFilters()
                .Groupable()
                .ClearFiltersButton(true)
                .Selectable(true)
                .SetStriped(true)
                .ChangePageSize(true)
                .WithGridItemsCount();

            return View(gridServer.Grid);


        }




        // GET: Movies/Details/5
        public async Task<IActionResult> Details([FromRoute]Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = new GetMovieDtoQuery { Id = id.Value };
            var result = await base.Mediator.Send(query, cancellationToken);

            return View(result);
        }

        // GET: Movies/Create
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var command = new CreateMovieDtoCommand();
            var result = await base.Mediator.Send(command, cancellationToken);

            result.Title = string.Empty;
            result.Rating = Ratings.Medium;
            result.ReleaseDate = DateTime.Now.Date;

            await this.InitMovieDtoNavigationProperties(null, result.GenreId, result.MediumTypeCode, result.Rating, cancellationToken);

            return View(result);
        }

        //// POST: Movies/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,GenreId,MediumTypeCode,Price,ReleaseDate,Rating")] Movie movie)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        movie.Id = Guid.NewGuid();
        //        _context.Add(movie);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        //    ViewData["MediumTypeCode"] = new SelectList(_context.MediumTypes, "Code", "Code", movie.MediumTypeCode);
        //    return View(movie);
        //}

        [HttpGet] // GET: Movies/Edit/5
        public async Task<ActionResult> Edit([FromRoute]Guid? id, CancellationToken cancellationToken)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var query = new GetMovieDtoQuery { Id = id.Value };
            var result = await base.Mediator.Send(query, cancellationToken);

            if(result == null)
            {
                return NotFound();
            }

            var model = new MovieModel { Movie = result };

            /* Genres, MediumTypes, Ratings für Dropdowns initialisieren */
            await this.InitMovieDtoNavigationProperties(model, result.GenreId, result.MediumTypeCode, result.Rating, cancellationToken);
                        
            return PartialView("_EditModal", result);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]Guid id, [Bind("Id,Title,GenreId,MediumTypeCode,Price,ReleaseDate,Rating")] MovieDto movieDto, CancellationToken cancellationToken)
        {
            if (id != movieDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var command = new UpdateMovieDtoCommand { Id = id, MovieDto = movieDto };
                    await base.Mediator.Send(command, cancellationToken);
                }
                catch 
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            var model = new MovieModel { Movie = movieDto };
            
            /* Genres, MediumTypes, Ratings für Dropdowns initialisieren */
            await this.InitMovieDtoNavigationProperties(model, movieDto.GenreId, movieDto.MediumTypeCode, movieDto.Rating, cancellationToken);
            
            return View(movieDto);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {

            if (id == null)
            {
                return NotFound();
            }

            var query = new GetMovieDtoQuery { Id = id.Value };
            var result = await base.Mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }
                                    
            return View(result);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteMovieDtoCommand { Id = id };
            await base.Mediator.Send(command, cancellationToken);
            
            return RedirectToAction(nameof(Index));
        }


        private async Task InitMovieDtoNavigationProperties(MovieModel? model, int? genreId = default, string mediumTypeCode = default, Ratings? rating = default, CancellationToken cancellationToken = default)
        {
            /* Genres, MediumTypes, Ratings für Dropdowns abrufen */

            var genres = HttpContext.Session.Get<IEnumerable<Genre>>(nameof(Genre));
            if (genres == null)
            {
                genres = await base.Mediator.Send(new GetGenresQuery(), cancellationToken);
                HttpContext.Session.Set<IEnumerable<Genre>>(nameof(genres), genres);
            }
            var genreSelectList = new SelectList(genres, nameof(Genre.Id), nameof(Genre.Name), genreId);

            var mediumTypes = HttpContext.Session.Get<IEnumerable<MediumType>>(nameof(MediumType));
            if (mediumTypes == null)
            {
                mediumTypes = await base.Mediator.Send(new GetMediumTypesQuery(), cancellationToken);
                HttpContext.Session.Set<IEnumerable<MediumType>>(nameof(MediumType), mediumTypes);
            }
            var mediumTypeSelectList = new SelectList(mediumTypes, nameof(MediumType.Code), nameof(MediumType.Name), mediumTypeCode);

            var localizedRatings = HttpContext.Session.Get<IEnumerable<KeyValuePair<object, string>>>(nameof(Ratings));
            if (localizedRatings == null)
            {
                localizedRatings = EnumExtensions.EnumToList<Ratings>();
                HttpContext.Session.Set<IEnumerable<KeyValuePair<object, string>>>(nameof(Ratings), localizedRatings);
            }
            var localizedRatingSelectList = new SelectList(localizedRatings, "Key", "Value", rating);


            /* ViewData["XY"] == ViewBag.XY */
            ViewData[nameof(Genre)] = genreSelectList;
            ViewBag.MediumType = mediumTypeSelectList;
            ViewBag.Ratings = localizedRatingSelectList;


            if (model != null)
            {
                model.Genre = genreSelectList;
                model.MediumType = mediumTypeSelectList;
                model.Ratings = localizedRatingSelectList;
            }

        }
    }
}
