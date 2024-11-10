using System;
using System.ComponentModel.DataAnnotations;

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

        public int Uid { get; init; }

        public User_detail(int uid, string username, string password)
        {
            Uid = uid;
            Username = username; // Validates through property setter
            Password = password; // Validates through property setter
        }
    }
}
