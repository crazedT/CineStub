using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CineStub.Model;
using CineStub.Service;

namespace CineStub.Web.DTOs
{
    public class SlotGroupDto
    {
        public SlotGroupDto(SlotGroup slotGroup)
        {
            DateTime = slotGroup.DateTime;

            Slots = new List<SlotDto>();

            foreach (var slot in slotGroup.Slots)
            {
                Slots.Add(new SlotDto(slot));
            }
        }

        public DateTime DateTime { get; set; }

        public List<SlotDto> Slots { get; set; }

        public string DayOfWeek
        {
            get { return DateTime.DayOfWeek.ToString(); }
        }
    }
}