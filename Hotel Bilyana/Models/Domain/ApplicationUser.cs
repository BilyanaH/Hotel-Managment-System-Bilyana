using Microsoft.AspNetCore.Identity;

namespace Hotel_Bilyana.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EGN { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
