﻿using System.ComponentModel.DataAnnotations;

namespace OnlineStoreApp.Models
{
    public enum UserType { Administrator, Seller, Buyer }
    public class User : BaseClass
    {
        [Required, MaxLength(100)]
        public string? Username { get; set; }
        [Required, MaxLength(300)]
        public string? Password { get; set; }
        [Required, MaxLength(100)]
        public string? Email { get; set; }
        [Required, MaxLength(100)]
        public string? FullName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required, MaxLength(200)]
        public string? Address { get; set; }
        [Required]
        public UserType Type { get; set; }
        public byte[]? Image { get; set; }
    }
}
