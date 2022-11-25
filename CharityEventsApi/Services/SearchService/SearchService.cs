﻿using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.FundraisingService;
using CharityEventsApi.Services.VolunteeringService;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CharityEventsApi.Services.SearchService
{
    public class SearchService: ISearchService
    {
        
        private readonly CharityEventsDbContext dbContext;

        public SearchService(CharityEventsDbContext dbContext, IFundraisingService fundraisingService, IVolunteeringService volunteeringService)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<GetAllDetailsCharityEventDto> GetCharityEvents(bool? isVerified, bool? isActive, bool? isFundraising, bool? isVolunteering,
           bool? volunteeringIsActive, bool? fundraisingIsActive, bool? volunteeringIsVerified, bool? fundraisingIsVerified)
        {
            var charityEvents = dbContext.Charityevents
                .Where(c => isVerified == null || c.IsVerified == Convert.ToSByte(isVerified))
                .Where(c => isActive == null || c.IsActive == Convert.ToSByte(isActive))
                .Where(c => isFundraising == null || c.CharityFundraisingIdCharityFundraising.Equals(null) == !isFundraising)
                .Where(c => isVolunteering == null || c.VolunteeringIdVolunteering.Equals(null) == !isVolunteering)
                .Where(c => c.VolunteeringIdVolunteeringNavigation == null || c.VolunteeringIdVolunteeringNavigation.IsActive == Convert.ToSByte(volunteeringIsActive))
                .Where(c => c.CharityFundraisingIdCharityFundraisingNavigation == null || c.CharityFundraisingIdCharityFundraisingNavigation.IsActive == Convert.ToSByte(fundraisingIsActive))
                .Where(c => c.VolunteeringIdVolunteeringNavigation == null || c.VolunteeringIdVolunteeringNavigation.IsVerified == Convert.ToSByte(volunteeringIsVerified))
                .Where(c => c.CharityFundraisingIdCharityFundraisingNavigation == null || c.CharityFundraisingIdCharityFundraisingNavigation.IsVerified == Convert.ToSByte(fundraisingIsVerified))
                .Include(c => c.CharityFundraisingIdCharityFundraisingNavigation)
                .Include(c => c.VolunteeringIdVolunteeringNavigation);

            var charityEventsDetails = new List<GetAllDetailsCharityEventDto>();

            foreach (Charityevent charityEvent in charityEvents)
                charityEventsDetails.Add(getDetails(charityEvent));

            return charityEventsDetails;
        }

        public GetAllDetailsCharityEventDto GetCharityEventsById(int charityEventId)
        {
            var charityEvent = dbContext.Charityevents
                .Where(c => c.IdCharityEvent == charityEventId)
                .Include(x => x.VolunteeringIdVolunteeringNavigation)
                .Include(x => x.CharityFundraisingIdCharityFundraisingNavigation);

            return getDetails(charityEvent.First());
        }

        public GetAllDetailsCharityEventDto getDetails(Charityevent charityEvent)
        {
            GetAllDetailsCharityEventDto charityEventDetails = new GetAllDetailsCharityEventDto()
            {
                IdCharityEvent = charityEvent.IdCharityEvent,
                IsActive = charityEvent.IsActive,
                Description = charityEvent.Description,
                FundraisingId = charityEvent.CharityFundraisingIdCharityFundraising,
                isVerified = charityEvent.IsVerified,
                Title = charityEvent.Title,
                VolunteeringId = charityEvent.VolunteeringIdVolunteering
            };

            if (charityEvent.VolunteeringIdVolunteeringNavigation != null)
            {
                charityEventDetails.CharityEventVolunteering = new GetCharityEventVolunteeringDto
                {
                    AmountOfNeededVolunteers = charityEvent.VolunteeringIdVolunteeringNavigation.AmountOfNeededVolunteers,
                    CreatedEventDate = charityEvent.VolunteeringIdVolunteeringNavigation.CreatedEventDate,
                    EndEventDate = charityEvent.VolunteeringIdVolunteeringNavigation.EndEventDate,
                    IsActive = charityEvent.VolunteeringIdVolunteeringNavigation.IsActive,
                    isVerified = charityEvent.VolunteeringIdVolunteeringNavigation.IsVerified,
                    Id = charityEvent.VolunteeringIdVolunteeringNavigation.IdVolunteering
                };
            }

            if (charityEvent.CharityFundraisingIdCharityFundraisingNavigation != null)
            {
                charityEventDetails.CharityEventFundrasing = new GetCharityFundrasingDto
                {
                    AmountOfAlreadyCollectedMoney = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.AmountOfAlreadyCollectedMoney,
                    AmountOfMoneyToCollect = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.AmountOfMoneyToCollect,
                    CreatedEventDate = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.CreatedEventDate,
                    EndEventDate = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.EndEventDate,
                    FundTarget = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.FundTarget,
                    IsActive = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IsActive,
                    isVerified = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IsVerified,
                    Id = charityEvent.CharityFundraisingIdCharityFundraisingNavigation.IdCharityFundraising
                };
            }

            return charityEventDetails;
        }

    }
}