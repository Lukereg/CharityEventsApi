﻿namespace CharityEventsApi.Models
{
    public class RegisterUserDto
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
