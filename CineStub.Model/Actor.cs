using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineStub.Model
{
    public class Actor : Entity
    {
        [Required]
        public int TmdbId { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageBaseUrl { get; set; }

        public string ProfilePath { get; set; }

        public string ProfileUrlW185
        {
            get { return ImageBaseUrl + "w185" + ProfilePath; }
        }

        public ICollection<ActorIndex> ActorIndices { get; set; }
    }
}
