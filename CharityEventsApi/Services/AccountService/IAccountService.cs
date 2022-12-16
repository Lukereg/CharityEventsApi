﻿using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.AccountService
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginUserDto dto);
        public void GiveRole(int idUser, string role);
    }
}
