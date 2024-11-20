using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ForJSON
{
  public class Receipt
  {
    public int UserId { get; set; }
    public int? CartId { get; set; }

    public List<int> Items { get; set; } = new List<int>();
    public DateTime CreatedAt { get; set; }
    public char PaymentMethod { get; set; }
    public string? Description { get; set; }

    public int TotalPrice { get; set; }
  }

}
