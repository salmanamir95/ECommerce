using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Category
    {
        [Key] // Marks this property as the primary key
        public int Cid { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters.")]
        public string CategoryName { get; set; } = string.Empty;

        //[ForeignKey("Item")]
        public List<Item> category_items { get; set; }= new List<Item>();
    }
}
