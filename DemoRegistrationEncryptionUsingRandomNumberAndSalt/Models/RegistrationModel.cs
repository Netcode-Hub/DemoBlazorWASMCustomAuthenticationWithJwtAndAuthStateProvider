﻿using System.ComponentModel.DataAnnotations;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Models
{
    public class RegistrationModel
    {
        [Required]
        public  string? Name { get; set; }
        [Required, EmailAddress]
        public  string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public  string? Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
