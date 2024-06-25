﻿using System.ComponentModel.DataAnnotations;
namespace APICatalogo.DTOs.Autentication
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
