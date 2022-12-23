﻿using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.ImageService;
using CharityEventsApi.Services.AuthUserService;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.VolunteeringService
{
    public class VolunteeringService : IVolunteeringService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly VolunteeringActivation volunteeringActication;
        private readonly VolunteeringVerification volunteeringVerification;
        private readonly CharityEventVerification charityEventVerification;
        private readonly VolunteeringDenial volunteeringDenial;

        public VolunteeringService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade,
            VolunteeringActivation volunteeringActication, VolunteeringVerification volunteeringVerification,
            CharityEventVerification charityEventVerification, VolunteeringDenial volunteeringDenial)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.volunteeringActication = volunteeringActication;
            this.volunteeringVerification = volunteeringVerification;
            this.charityEventVerification = charityEventVerification;
            this.volunteeringDenial = volunteeringDenial;
        }

        public async Task Add(AddCharityEventVolunteeringDto dto)
        {
            var charityevent = dbContext.CharityEvents.FirstOrDefault(f => f.IdCharityEvent == dto.IdCharityEvent);
            if (charityevent is null) 
            {
                throw new NotFoundException("Charity event with given id doesn't exist");
            }
            if(charityevent.IdCharityVolunteering is not null)
            {
                throw new BadRequestException("Can't add charity event volunteering, because another one already exists in this charity event");
            }
            await charityEventFactoryFacade.AddCharityEventVolunteering(dto, charityevent);

            charityEventVerification.SetValue(dto.IdCharityEvent, false);
        }
        public void SetActive(int idVolunteering, bool isActive)
        {
            volunteeringActication.SetValue(idVolunteering, isActive);
        }
        public void SetVerify(int idVolunteering, bool isVerified)
        {
            volunteeringVerification.SetValue(idVolunteering, isVerified);
        }
        public void SetDeny(int idVolunteering, bool isDenied)
        {
            volunteeringDenial.SetValue(idVolunteering, isDenied);
        }
        public void Edit(EditCharityEventVolunteeringDto VolunteeringDto, int idVolunteering)
        {
            var charityevent = getVolunteeringByIdVolunteering(idVolunteering);

            if (VolunteeringDto.AmountOfNeededVolunteers != null)
            {
                charityevent.AmountOfNeededVolunteers = (int)VolunteeringDto.AmountOfNeededVolunteers;
            }
            
            dbContext.SaveChanges();
        }
      
        public GetCharityEventVolunteeringDto GetById(int idVolunteering)
        {
            var c = getVolunteeringByIdVolunteering(idVolunteering);

            return new GetCharityEventVolunteeringDto
            {
                Id = c.IdCharityVolunteering,
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                IsVerified = c.IsVerified
            };
        }
        public IEnumerable<GetCharityEventVolunteeringDto> GetAll()
        {
            var volunteerings = dbContext.CharityVolunteerings.Select(c => new GetCharityEventVolunteeringDto
            {
                Id = c.IdCharityVolunteering,
                AmountOfNeededVolunteers = c.AmountOfNeededVolunteers,
                CreatedEventDate = c.CreatedEventDate,
                EndEventDate = c.EndEventDate,
                IsActive = c.IsActive,
                IsVerified = c.IsVerified
            }
            );

            return volunteerings;
        }
        private CharityVolunteering getVolunteeringByIdVolunteering(int idVolunteering)
        {
            var volunteering = dbContext.CharityVolunteerings.FirstOrDefault(c => c.IdCharityVolunteering == idVolunteering);
            if (volunteering is null)
            {
                throw new NotFoundException("Charity event volunteering with given id doesn't exist");
            }

            return volunteering;
        }

       
    }
}
