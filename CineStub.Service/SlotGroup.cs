using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineStub.Model;

namespace CineStub.Service
{
    public class SlotGroup
    {
        public DateTime DateTime { get; set; }

        public List<Slot> Slots { get; set; }

        public string DayOfWeek
        {
            get { return DateTime.DayOfWeek.ToString(); }
        }
    }
}
