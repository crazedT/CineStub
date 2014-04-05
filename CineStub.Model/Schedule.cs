using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CineStub.Model
{
    public class Schedule : Entity
    {
        public Schedule()
        {

        }

        public Schedule(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;

            InitializeSlots(startDate, endDate);
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IList<Slot> Slots { get; set; }

        private void InitializeSlots(DateTime startDate, DateTime endDate)
        {
            Slots = new List<Slot>();
            var tempDate = startDate;

            var hours = new List<int>();
            for (var i = 9; i <= 23; i++)
            {
                hours.Add(i);
            }

            while (tempDate.Date != endDate.Date.AddDays(1))
            {
                foreach (var hour in hours)
                {
                    Slots.Add(new Slot()
                    {
                        DateTime = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, hour, 0, 0),
                        IsOpen = true
                    });
                }

                tempDate = tempDate.Date.AddDays(1);
            }
        }
    }
}

