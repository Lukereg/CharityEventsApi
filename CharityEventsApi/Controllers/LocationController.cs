﻿using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.LocationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharityEventsApi.Controllers
{

    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController: ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost()]
        public ActionResult AddLocation([FromBody] AddLocationDto addLocationDto)
        {
            locationService.addLocation(addLocationDto);
            return Ok();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{locationId}")]
        public ActionResult EditLocation([FromBody] EditLocationDto locationDto, [FromRoute] int locationId)
        {
            locationService.editLocation(locationDto, locationId);
            return Ok();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("{locationId}")]
        public ActionResult DeleteLocation([FromRoute] int locationId)
        {
            locationService.deleteLocation(locationId);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{locationId}")]
        public ActionResult GetLocationById([FromRoute] int locationId)
        {
            return Ok(locationService.getLocationById(locationId));
        }

        [AllowAnonymous]
        [HttpGet("charityEventVolunteering/{volunteeringId}")]
        public ActionResult GetLocationsByCharityEventVolunteeringId([FromRoute] int volunteeringId)
        {
            return Ok(locationService.getLocationsByVolunteeringId(volunteeringId));
        }
    }
}
