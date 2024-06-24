﻿using Microsoft.AspNetCore.Identity;

namespace OrganizeApp.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
