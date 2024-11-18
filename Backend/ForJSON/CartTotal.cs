using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ForJSON
{
  public class CartTotal
  {
    [Required(ErrorMessage = "User ID is required.")]
    public int Userid { get; set; }

    [Required(ErrorMessage = "Cart ID is required.")]
    public int cartId { get; set; }

  }
}
