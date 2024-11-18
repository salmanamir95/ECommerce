using System.ComponentModel.DataAnnotations;  // <-- Add this

public class CartUserInfo
{
    [Required(ErrorMessage = "User ID is required.")]
    public int Userid { get; set; }

    [Required(ErrorMessage = "Cart ID is required.")]
    public int cartId { get; set; }

    [Required(ErrorMessage = "Item ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Item ID must be a positive integer.")]
    public int Itemid { get; set; }
}
