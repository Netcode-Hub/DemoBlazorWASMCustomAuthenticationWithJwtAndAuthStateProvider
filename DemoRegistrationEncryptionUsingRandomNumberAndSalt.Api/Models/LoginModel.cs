﻿using System.ComponentModel.DataAnnotations;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models
{
    public class LoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
