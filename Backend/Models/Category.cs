using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Category
    {
        [Key] // Marks this property as the primary key
        public int Cid { get; set; } // Category ID (Primary key)

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters.")]
        public string CategoryName { get; set; } = string.Empty; // Name of the Category

        // Navigation property for the related Items (one-to-many relationship)
        public List<Item> category_items { get; set; } = new List<Item>(); // A Category has many Items
    }
}
