using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace APi.Entities
{
    public class AppRole :IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
