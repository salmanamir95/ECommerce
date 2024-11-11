using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Item
    {
        [Key] // Marks this property as the primary key
        public int Iid { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Item name must be between 2 and 100 characters.")]
        public string ItemName { get; set; } = string.Empty; // Updated to PascalCase

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a non-negative number.")]
        public int Price { get; set; }

        [ForeignKey("Category")] // Indicates that this is a foreign key referencing the Category table
        public int Cid { get; set; }

        // Navigation property for the related Category
        public virtual Category? Category { get; set; }
    }
}
