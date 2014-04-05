using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CineStub.Model;

namespace CineStub.Web.DTOs
{
    public class SlotDto
    {
        public SlotDto(Slot slot)
        {
            SlotId = slot.Id;
            IsOpen = slot.IsOpen;
            IsRoot = slot.IsRoot;

            if (slot.Movie != null)
            {
                MovieId = slot.Movie.Id;
                MovieTitle = slot.Movie.Title;
            }

            DateTime = slot.DateTime;
        }

        public DateTime DateTime { get; set; }

        public bool IsOpen { get; set; }

        public bool IsRoot { get; set; } 

        public int SlotId { get; set; }

        public int MovieId { get; set; }

        public string MovieTitle { get; set; }
    }
}