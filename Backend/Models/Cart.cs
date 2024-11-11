using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Cart
    {
        [Key]
        public int cid { get; set; }

        [ForeignKey("User_detail")]
        [Required(ErrorMessage = "User ID is required")]
        public int Uid { get; set; }

        [ForeignKey("Item")]
        [Required(ErrorMessage = "Item ID is required")]
        public int Iid { get; set; }

        public virtual User_detail? user { get; set; }

        public virtual Item? item { get; set; }
    }
}
