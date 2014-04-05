using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineStub.Model
{
    public class Customer : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
