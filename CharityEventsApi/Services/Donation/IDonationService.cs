﻿using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.Donation
{
    public interface IDonationService
    {
        public void addDonation(AddDonationDto addDonationDto);

        public GetDonationDto getDonationById(int donationId);
    }
}
