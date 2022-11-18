﻿using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.CharityEventService
{
    public class CharityEventService : ICharityEventService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly ICharityEventFactoryFacade charityEventFactoryFacade;
        private readonly CharityEventVerification charityEventVerification;
        private readonly CharityEventActivation charityEventActivation;

        public CharityEventService(CharityEventsDbContext dbContext, ICharityEventFactoryFacade charityEventFactoryFacade, 
            CharityEventVerification charityEventVerification, CharityEventActivation charityEventActivation)
        {
            this.dbContext = dbContext;
            this.charityEventFactoryFacade = charityEventFactoryFacade;
            this.charityEventVerification = charityEventVerification;
            this.charityEventActivation = charityEventActivation;
        }

        public void Add(AddAllCharityEventsDto charityEventDto)
        {
           charityEventFactoryFacade.AddCharityEvent(charityEventDto);
        }
        public void getAll()
        {
            var charityevent = dbContext.Charityevents;
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
        }
        public void Edit(EditCharityEventDto charityEventDto, int charityEventId)
        {
           var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if(charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityevent.Title = charityEventDto.Title;
            charityevent.Description = charityEventDto.Description;
            dbContext.SaveChanges();
        }

     

        public void SetActive(int chariyEventId, bool isActive)
        {
            charityEventActivation.SetActive(chariyEventId, isActive);
        }
     
        public void SetVerify(int chariyEventId, bool isVerified)
        {
            charityEventVerification.SetVerify(chariyEventId, isVerified);
        }
        public GetCharityEventDto GetCharityEventById(int id)
        {
            var c = dbContext.Charityevents.FirstOrDefault(c => c.IdCharityEvent == id);
            if(c is null)
            {
                throw new NotFoundException("Given id doesn't exist");
            }

            return new GetCharityEventDto { 
                Description = c.Description,
                IsActive = c.IsActive,
                Title = c.Title,
                VolunteeringId = c?.VolunteeringIdVolunteering,
                FundraisingId = c?.CharityFundraisingIdCharityFundraising,
                isVerified = c.IsVerified
            };
        }
      

    }
}