using System.ComponentModel.DataAnnotations;

namespace JWTToken_Auth_DAL.Dto
{
    public class RegisterModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "AccountType is required")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "PhoneNo is required")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}
