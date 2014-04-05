using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineStub.Model
{
    public class Movie : Entity
    {
        [Required]
        public int TmdbId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Genres { get; set; }

        public string Overview { get; set; }

        public int Runtime { get; set; }

        public string YoutubeTrailerSource { get; set; }

        public string YoutubeTrailerUrl
        {
            get { return "http://www.youtube.com/watch?v=" + YoutubeTrailerSource; }
        }

        public MovieCast MovieCast { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public ICollection<MovieImage> MovieImages { get; set; }

        public string ImageBaseUrl { get; set; }

        public string PosterPath { get; set; }

        public string BackdropPath { get; set; }

        public string PosterUrlW92
        {
            get { return ImageBaseUrl + "w92" + PosterPath; }
        }

        public string PosterUrlW154
        {
            get { return ImageBaseUrl + "w154" + PosterPath; }
        }

        public string PosterUrlW342
        {
            get { return ImageBaseUrl + "w342" + PosterPath; }
        }

        public string PosterUrlW780
        {
            get { return ImageBaseUrl + "w780" + PosterPath; }
        }

        public string BackdropUrlW780
        {
            get { return ImageBaseUrl + "w780" + BackdropPath; }
        }

        public ICollection<Slot> Slots { get; set; }
    }
}
