﻿using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Models.Validators
{
    public class AddCharityEventVolunteeringDtoValidator : AbstractValidator<AddCharityEventVolunteeringDto>
    {
        public AddCharityEventVolunteeringDtoValidator(CharityEventsDbContext dbContext)
        {
            RuleFor(x => x.CharityEventId)
               .NotEmpty()
               .Custom((value, context) =>
               {
                   var charityEventExist = dbContext.Charityevents.Any(c => c.IdCharityEvent == value);

                   if (!charityEventExist)
                       context.AddFailure("CharityEventId", "Nie istnieje wydarzenie o podanym id");
               });

            RuleFor(x => x.AmountOfNeededVolunteers)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}