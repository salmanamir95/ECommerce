using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Cart
    {
        public int user_id { get; set; }

        public List<CartItem>? Products { get; set; }= new List<CartItem>();

    }
}