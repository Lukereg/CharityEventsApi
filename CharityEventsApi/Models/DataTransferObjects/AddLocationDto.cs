﻿namespace CharityEventsApi.Models.DataTransferObjects
{
    public class AddLocationDto
    {
        public int IdCharityVolunteering { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;
    }
}
