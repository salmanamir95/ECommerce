using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Backend.Models
{
    public class User_detail
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
        public int Uid { get; init; }

        // Navigation property for the related Cart (one-to-one relationship)
        public virtual Cart? Cart { get; set; }  // One User has One Cart

        // Navigation property for the related User_Other_Info (one-to-one relationship)
        public virtual User_Other_Info? UserOtherInfo { get; set; }

        // Navigation property for the related Orders (one-to-many relationship)
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
