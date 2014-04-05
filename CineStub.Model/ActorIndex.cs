using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineStub.Model
{
    public class ActorIndex : Entity
    {
        public int MovieCastId { get; set; }
        public MovieCast MovieCast { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public int Index { get; set; }
    }
}
