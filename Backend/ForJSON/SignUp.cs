using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ForJSON
{
    public class SignUp
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username
        {
            get => _username;
            set => _username = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Username cannot be empty.");
        }
        private string _username = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password
        {
            get => _password;
            set => _password = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Password cannot be empty.");
        }
        private string _password = string.Empty;


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
