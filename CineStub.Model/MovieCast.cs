using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineStub.Model
{
    public class MovieCast : Entity
    {
        public MovieCast()
        {
            ActorIndices = new List<ActorIndex>();
        }

        public ICollection<ActorIndex> ActorIndices { get; set; }

        [Required]
        public Movie Movie { get; set; }
    }
}
