﻿namespace ProductAPI.DTO.User
{
    public class BuyerRegistrationPostDto : UserBaseDto
    {
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}