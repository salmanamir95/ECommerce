using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Cart
    {
        [Key]
        public int Pid { get; set; }

        public int cid { get; set; } // cartid

        [ForeignKey("User_detail")]
        public virtual User_detail? user { get; set; }

        public virtual List<Item> item { get; set; } = new List<Item>(); // Collection of items

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
