using APi.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace APi.Entities
{
    public class AppUser :IdentityUser<int>
    {
      
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; } 
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        // public int GetAge() => DateOfBirth.CalaculateAge();
        public ICollection<UserLike> LikeByUser { get; set; }
        public ICollection<UserLike> LikedUser { get; set; } 
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesRecived { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }


    }
}
