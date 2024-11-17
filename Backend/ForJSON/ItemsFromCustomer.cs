using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.ForJSON
{
    public class ItemsFromCustomer
    {
        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, ErrorMessage = "Item name can't exceed 100 characters.")]
        public string item { get; set; } = string.Empty;

        [Required(ErrorMessage = "Item ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Item ID must be a positive integer.")]
        public int Iid { get; set; }

        public int Uid { get; set; }
        public int cId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a non-negative integer.")]
        public int price { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public Category _category { get; set; }
    }
}
