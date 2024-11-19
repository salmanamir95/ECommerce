using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Order
    {
        [Key]
        public int Oid { get; set; }

        // Foreign Key for User_detail
        [ForeignKey("User_detail")]
        public int Uid { get; set; }  // This is the foreign key for User_detail

        public virtual User_detail? user { get; set; } // Navigation property for User_detail

        // Foreign Key for Cart (existing relationship)
        [ForeignKey("Cart")]
        public int? CartId { get; set; }  // Assuming CartId is the primary key of Cart

        public virtual Cart? cart { get; set; } // Navigation property for Cart

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int IID { get; set; }

        public Item? item { get; set; }

        public string? Desc { get; set; }

        public char PaymentMethod { get; set; }
    }
}
