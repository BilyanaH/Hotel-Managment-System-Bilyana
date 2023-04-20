using System.ComponentModel.DataAnnotations;

namespace Hotel_Bilyana.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }       
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
    
        [Required]
        public string PhoneNumber { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "EGN cannot be less than 10 digits")]
        [Required]
        public string EGN { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$",ErrorMessage ="Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string? Role { get; set; }
        
    }
}
