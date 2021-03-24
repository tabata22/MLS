using System;
using Microsoft.AspNetCore.Identity;

namespace MLS.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string PersonalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
