using Microsoft.AspNetCore.Identity;

namespace JWTToken_Auth_DAL.Model
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public bool IsDeleted { get; set; }

    }
}
