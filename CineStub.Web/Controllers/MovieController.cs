using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using CineStub.Data;
using CineStub.Model;
using CineStub.Service;
using CineStub.Web.DTOs;
using CineStub.Web.Helpers;
using WebGrease.Css.Extensions;

namespace CineStub.Web.Controllers
{
    [RequireHttps]
    public class MovieController : ApiController
    {
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        // GET api/movie
        public IEnumerable<Movie> Get()
        {
            return _movieService.AllMovies;
        }

        [System.Web.Http.Route("api/movies/current")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Movie> GetCurrent()
        {
            return _movieService.GetCurrentMovies();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/movie/showtimes/{movieId}")]
        public IEnumerable<SlotGroupDto> GetShowtimes(int movieId)
        {
            var slotGroups = _movieService.GetMovieShowtimes(movieId);

            return slotGroups.Select(slotGroup => new SlotGroupDto(slotGroup)).ToList();
        }

        // GET api/movie/5
        public Movie Get(int id)
        {
            return _movieService.GetMovieDetails(id);
        }

        // POST api/movie
        [System.Web.Http.Authorize]                     // todo: restrict to admin
        public void Post([FromBody]int tmdbId)
        {
            var movie = TmdbHelper.GetMovieFromTmdbId(tmdbId);

            try
            {
                _movieService.AddMovieGraph(movie);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    ReasonPhrase = e.Message
                });
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/movie/searchtmdb/{title}")]
        public IEnumerable<Movie> SearchTmdb(string title)
        {
            return TmdbHelper.SearchByTitle(title);
        }
    }
}
