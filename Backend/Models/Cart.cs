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

        public List<Item> Items { get; set; }= new List<Item>();

        public List<Order> orders { get; set; }= new List<Order>();

        public virtual User_detail? user { get; set; }

        // public virtual Item? item { get; set; }
    }
}
