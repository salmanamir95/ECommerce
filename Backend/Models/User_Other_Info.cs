using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class User_Other_Info
    {
        // The Uid in User_Other_Info will be both the foreign key and the primary key
        [Key]
        [ForeignKey("User_detail")]
        public int Uid { get; init; } // Uid as both Primary Key and Foreign Key

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be in a valid international format (e.g., +123456789).")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 15 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        // Navigation property for the related User_detail
        public virtual User_detail UserDetail { get; set; }
    }
}
