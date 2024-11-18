using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ForJSON
{
    public class ReturnQuantityInfoPerItem
    {
        public CartTotal UserCartInfo { get; set; }

        public int itemId {get; set;}

        public int quantity { get; set; }
    }
}
