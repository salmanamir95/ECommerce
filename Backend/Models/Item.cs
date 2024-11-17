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
        public string ItemName { get; set; } = string.Empty; // Name of the Item

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Quantity of the Item

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a non-negative number.")]
        public int Price { get; set; } // Price of the Item

        [ForeignKey("Cart")] // Foreign key referencing the Cart table
        public int CartId { get; set; }  // Cart foreign key

        public virtual Cart Cart { get; set; }  // Navigation property to Cart

        [ForeignKey("Category")] // Foreign key referencing the Category table
        public int Cid { get; set; } // Category foreign key for the Item

        public virtual Category Category { get; set; }  // Navigation property to Category (Many Items belong to one Category)
    }
}
