using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.ForJSON
{
    public class ItemsFromCustomer : IValidatableObject
    {
        [Required(ErrorMessage = "Item ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Item ID must be a positive integer.")]
        public int Itemid { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive integer.")]
        public int Userid { get; set; }

        [Required(ErrorMessage = "Cart ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Cart ID must be a positive integer.")]
        public int cartId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int quantity { get; set; }

        // Custom validation logic (optional)
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Example: Check if Userid and CartId match a valid existing cart or user (this is a sample check, adjust as per your logic)
            if (Userid <= 0)
            {
                results.Add(new ValidationResult("User ID must be valid.", new[] { "Userid" }));
            }

            if (cartId <= 0)
            {
                results.Add(new ValidationResult("Cart ID must be valid.", new[] { "cartId" }));
            }

            // Check if Itemid exists in your database (this would typically require a service call)
            // You can use an async service check if necessary, or just simulate it here
            if (Itemid <= 0)
            {
                results.Add(new ValidationResult("Item ID is invalid or not found.", new[] { "Itemid" }));
            }

            // You could also perform more business-specific validations, such as checking if
            // the requested quantity exceeds available stock
            // For example:
            if (quantity > 1000)  // Assuming 1000 is the max allowable quantity
            {
                results.Add(new ValidationResult("Quantity exceeds the maximum limit of 1000.", new[] { "quantity" }));
            }

            return results;
        }
    }
}
