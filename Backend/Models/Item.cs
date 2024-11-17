using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Item
    {
        [Key] // Primary key for Item
        public int Iid { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Item name must be between 2 and 100 characters.")]
        public string ItemName { get; set; } = string.Empty; // Using PascalCase

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a non-negative number.")]
        public int Price { get; set; }

        // Foreign key property referencing the Cart table

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; } // Navigation property for Cart

        [ForeignKey("Category")] // Indicates a foreign key referencing Category
        public int Cid { get; set; }
    }
}
