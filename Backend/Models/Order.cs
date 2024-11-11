using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Order
    {
        [Key]
        public int Oid { get; set; }

        [ForeignKey("User_detail")]
        [Required(ErrorMessage = "User ID is required")]
        public int Uid { get; set; }

        [ForeignKey("Cart")]
        [Required(ErrorMessage = "Cart ID is required")]
        public int Cid { get; set; }

        public virtual User_detail? user { get; set; }

        public virtual Cart? cart { get; set; }
    }
}
