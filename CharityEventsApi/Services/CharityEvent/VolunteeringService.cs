﻿using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEvent
{
    public class VolunteeringService : IVolunteeringService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly VolunteeringFactory charityEventVolunteeringFactory;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly VolunteeringActivation volunteeringActication;
        private readonly VolunteeringVerification volunteeringVerification;

        public VolunteeringService(CharityEventsDbContext dbContext, VolunteeringFactory charityEventVolunteeringFactory, ICharityEventFactoryFacade charityEventFactoryFacade,
            VolunteeringActivation volunteeringActication, VolunteeringVerification volunteeringVerification)
        {
            this.dbContext = dbContext;
            this.charityEventVolunteeringFactory = charityEventVolunteeringFactory;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.volunteeringActication = volunteeringActication;
            this.volunteeringVerification = volunteeringVerification;
        }
        [Obsolete("AddLocation is deprecated, please use location controller instead")]
        public void AddLocation(AddLocationDto locationDto)
        {
            charityEventFactoryFacade.AddLocation(locationDto);
        }

        [Obsolete("EditLocation is deprecated, please use location controller instead")]
        public void EditLocation(EditLocationDto locationDto, int locationId)
        {
            var location = dbContext.Locations.FirstOrDefault(l => l.IdLocation == locationId);
            if (location == null)
            {
                throw new NotFoundException("Location with given id doesn't exist");
            }
            location.Street = locationDto.Street;
            location.PostalCode = locationDto.PostalCode;
            location.Town = locationDto.Town;
            dbContext.SaveChanges();

        }
        public void Add(AddCharityEventVolunteeringDto dto, int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(f => f.IdCharityEvent == charityEventId);
            if (charityevent is null)
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            Volunteering cv = charityEventVolunteeringFactory.CreateCharityEvent(dto);
            dbContext.Volunteerings.Add(cv);
            dbContext.SaveChanges();
        }
        public void SetActive(int VolunteeringId, bool isActive)
        {
            volunteeringActication.SetActive(VolunteeringId, isActive);
        }
        public void SetVerify(int VolunteeringId, bool isVerified)
        {
            volunteeringVerification.SetVerify(VolunteeringId, isVerified);
        }
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int VolunteeringId)
        {
            var charityevent = dbContext.Volunteerings.FirstOrDefault(v => v.IdVolunteering == VolunteeringId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEventVolunteering with given id doesn't exist");
            }
            //TODO: maybe throw warning in same way
            if (VolunteeringDto.AmountOfNeededVolunteers != null)
            {
                charityevent.AmountOfNeededVolunteers = (int)VolunteeringDto.AmountOfNeededVolunteers;
            }
            dbContext.SaveChanges();
        }
      

        public GetCharityEventVolunteeringDto GetById(int id)
        {
            var c = dbContext.Volunteerings.FirstOrDefault(c => c.IdVolunteering == id);
            if (c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }


            return new GetCharityEventVolunteeringDto
            {
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                isVerified = c.IsVerified
            };
        }

    }
}
