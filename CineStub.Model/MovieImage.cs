using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineStub.Model
{
    public class MovieImage : Entity
    {
        public string ImageBaseUrl { get; set; }

        public string ImagePath { get; set; }

        public string ImageUrlW1280
        {
            get { return ImageBaseUrl + "w1280" + ImagePath; }
        }


        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
