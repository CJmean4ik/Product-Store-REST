﻿namespace ProductAPI.DTO.User
{
    public class UserAuthorizePostDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EnteredPassword { get; set; } = string.Empty;
    }
}
