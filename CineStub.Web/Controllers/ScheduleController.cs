using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using CineStub.Data;
using CineStub.Model;
using CineStub.Service;
using CineStub.Web.DTOs;

namespace CineStub.Web.Controllers
{
    [RequireHttps]
    public class ScheduleController : ApiController
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // GET api/schedule
        public IEnumerable<Schedule> Get()
        {
            return _scheduleService.AllSchedules;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/schedule/slotsGroupedByDate/{scheduleId}")]
        public IEnumerable<SlotGroupDto> GetSlotsGroupedByDate(int scheduleId)
        {
            var slotsByDate = _scheduleService.SlotsByDate(scheduleId);

            return slotsByDate.Select(slotGroup => new SlotGroupDto(slotGroup)).ToList();
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/schedule/scheduleMovie")]
        public void ScheduleMovie([FromBody]SlotMovieIds ids)
        {
            try
            {
                _scheduleService.ScheduleMovie(ids.SlotId, ids.MovieId);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest){ReasonPhrase = e.Message});
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/schedule/unscheduleMovie/{rootSlotId}")]
        public void UnscheduleMovie(int rootSlotId)
        {
            _scheduleService.UnscheduleMovie(rootSlotId);
        }
        
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/schedule/addSchedule")]
        public Schedule AddSchedule()
        {
            return _scheduleService.AddSchedule();
        }
    }

    public class SlotMovieIds
    {
        public int SlotId { get; set; }
        public int MovieId { get; set; }
    }
}
