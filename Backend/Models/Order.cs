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
        public virtual User_detail? user { get; set; }

        [ForeignKey("Cart")]
        public virtual Cart? cart { get; set; }


    }
}
