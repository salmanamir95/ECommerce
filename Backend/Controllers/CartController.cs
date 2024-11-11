using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {

    }
}

// ### 1. `Cart.cs`
// - **AddItemToCart(Item item, int quantity)**: Adds an item to the cart.
// - **RemoveItemFromCart(Item item)**: Removes a specific item from the cart.
// - **UpdateItemQuantity(Item item, int quantity)**: Updates the quantity of a specific item in the cart.
// - **GetCartTotal()**: Calculates and returns the total cost of items in the cart.
// - **ClearCart()**: Clears all items in the cart.
// - **GetItemsInCart()**: Returns a list of all items currently in the cart.
