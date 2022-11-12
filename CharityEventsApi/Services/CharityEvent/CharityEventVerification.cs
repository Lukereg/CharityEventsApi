﻿using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.CharityEvent
{
    public class CharityEventVerification : VerificationBase
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly CharityEventActivation charityEventActivation;

        public CharityEventVerification(CharityEventsDbContext dbContext, CharityEventActivation charityEventActivation)
        {
            this.dbContext = dbContext;
            this.charityEventActivation = charityEventActivation;
        }
        protected override void verify(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityevent.IsVerified = 1;
            dbContext.SaveChanges();
        }
        protected override void unverify(int charityEventId)
        {
            var charityevent = dbContext.Charityevents.FirstOrDefault(ce => ce.IdCharityEvent == charityEventId);
            if (charityevent == null)
            {
                throw new NotFoundException("CharityEvent with given id doesn't exist");
            }
            charityEventActivation.SetActive(charityEventId, false);
            charityevent.IsVerified = 0;
            dbContext.SaveChanges();
        }
    }
}
