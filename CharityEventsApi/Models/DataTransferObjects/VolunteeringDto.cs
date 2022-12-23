﻿
namespace CharityEventsApi.Models.DataTransferObjects
{
    public class VolunteeringDto
    {
        public int IdCharityVolunteering { get; set; }
        public int AmountOfNeededVolunteers { get; set; }
        public DateTime CreatedEventDate { get; set; }
        public DateTime? EndEventDate { get; set; }
        public sbyte IsActive { get; set; }
        public sbyte IsVerified { get; set; }

    }
}
