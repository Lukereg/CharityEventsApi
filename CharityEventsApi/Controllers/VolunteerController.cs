﻿using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.LocationService;
using CharityEventsApi.Services.VolunteerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class VolunteerController: ControllerBase
    {
        private readonly IVolunteerService volunteerService;

        public VolunteerController(IVolunteerService volunteerService)
        {
            this.volunteerService = volunteerService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public ActionResult AddVolunteer([FromBody] AddVolunteerDto addVolunteerDto)
        {
            volunteerService.addVolunteer(addVolunteerDto);
            return Ok();
        }
    }
}