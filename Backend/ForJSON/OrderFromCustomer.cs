using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ForJSON
{
    public class OrderFromCustomer
    {

        public int Uid { get; set; }  // This is the foreign key for User_detail

        public int? CartId { get; set; }  // Assuming CartId is the primary key of Cart
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<int> IID { get; set; }= new List<int>();

        public string? Desc { get; set; }

        public char PaymentMethod { get; set; }

    }
}
