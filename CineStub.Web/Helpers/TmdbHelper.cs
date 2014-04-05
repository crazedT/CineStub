using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Configuration;
using CineStub.Model;
using WatTmdb.V3;

namespace CineStub.Web.Helpers
{
    public static class TmdbHelper
    {
        private static readonly Tmdb TmdbBase;
        private static readonly string ImageBaseUrl;

        static TmdbHelper()
        {
            TmdbBase = new Tmdb(WebConfigurationManager.AppSettings["TmdbApiKey"]);
            ImageBaseUrl = TmdbBase.GetConfiguration().images.secure_base_url;
        }

        public static IEnumerable<Movie> SearchByTitle(string title)
        {
            var movies = new List<Movie>();

            var movieSearch = TmdbBase.SearchMovie(title, 1);

            foreach (var movieResult in movieSearch.results)
            {
                movies.Add(new Movie()
                {
                    TmdbId = movieResult.id,
                    Title = movieResult.title,
                    ReleaseDate = ScraperUtilities.StringToNullableDateTime(movieResult.release_date),
                    ImageBaseUrl = ImageBaseUrl,
                    BackdropPath = movieResult.backdrop_path,
                    PosterPath = movieResult.poster_path
                });
            }

            return movies.OrderByDescending(m => m.ReleaseDate);
        }


        public static Movie GetMovieFromTmdbId(int tmdbId)
        {
            var tmdbMovie = TmdbBase.GetMovieInfo(tmdbId);
            var tmdbMovieTrailers = TmdbBase.GetMovieTrailers(tmdbId);
            var tmdbMovieImages = TmdbBase.GetMovieImages(tmdbId);

            var images = new List<MovieImage>();
            foreach (var backdrop in tmdbMovieImages.backdrops)
            {
                images.Add(new MovieImage(){ImageBaseUrl = ImageBaseUrl, ImagePath = backdrop.file_path});
            }

            var movie = new Movie()
            {
                ImageBaseUrl = ImageBaseUrl,
                TmdbId = tmdbId,
                Title = tmdbMovie.title,
                ReleaseDate = ScraperUtilities.StringToNullableDateTime(tmdbMovie.release_date),
                PosterPath = tmdbMovie.poster_path,
                BackdropPath = tmdbMovie.backdrop_path,
                Genres = String.Join(", ", tmdbMovie.genres.Select(movieGenre => movieGenre.name).ToArray()),
                Overview = tmdbMovie.overview,
                Runtime = tmdbMovie.runtime,
                YoutubeTrailerSource = tmdbMovieTrailers.youtube[0].source,
                MovieCast = GetMovieCast(tmdbId),
                MovieImages = images
            };

            return movie;
        }

        public static MovieCast GetMovieCast(int tmdbId)
        {
            var tmdbMovieCast = TmdbBase.GetMovieCast(tmdbId);

            var movieCast = new MovieCast();
            foreach (var castMember in tmdbMovieCast.cast.OrderBy(c => c.order))
            {
                movieCast.ActorIndices.Add(new ActorIndex()
                {
                    Index = castMember.order,
                    MovieCast = movieCast,
                    Actor = new Actor()
                    {
                        TmdbId = castMember.id,
                        ImageBaseUrl = ImageBaseUrl,
                        ProfilePath = castMember.profile_path,
                        Name = castMember.name
                    }
                });
            }

            return movieCast;
        }


        public static Dictionary<int, Actor> GetActorsWithOrderIndex(int tmdbId)
        {
            var actorIndexDictionary = new Dictionary<int, Actor>();

            var tmdbMovieCast = TmdbBase.GetMovieCast(tmdbId);
            foreach (var castMember in tmdbMovieCast.cast.OrderBy(c => c.order))
            {
                var actor = new Actor()
                {
                    TmdbId = castMember.id,
                    Name = castMember.name,
                    ImageBaseUrl = ImageBaseUrl,
                    ProfilePath = castMember.profile_path
                };

                actorIndexDictionary.Add(castMember.order, actor);
            }

            return actorIndexDictionary;
        }


        private static Movie TmdbMovieResultToMovieSummary(MovieResult movieResult)
        {
            var movie = new Movie
            {
                Title = movieResult.title,
                TmdbId = movieResult.id,
                PosterPath = movieResult.poster_path,
                ImageBaseUrl = ImageBaseUrl,
                ReleaseDate = ParseReleaseDate(movieResult.release_date)
            };

            return movie;
        }

        private static DateTime? ParseReleaseDate(string releaseDate)
        {
            DateTime? release;

            if (releaseDate != "")
            {
                release = DateTime.Parse(releaseDate);
            }
            else
            {
                release = null;
            }

            return release;
        }
    }
}