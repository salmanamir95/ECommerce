using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class User_Other_Info
    {
        [ForeignKey("User_detail")]
        public int Uid { get; init; }

        [Key]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be in a valid international format (e.g., +123456789).")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 15 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
