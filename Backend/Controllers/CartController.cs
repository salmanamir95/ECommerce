using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.ForJSON;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CartController : ControllerBase
  {

    private readonly ApplicationDBContext _context;
    private readonly string? _conn;
    private readonly IConfiguration _configuration;  // Declare IConfiguration

    // Inject IConfiguration into the constructor
    public CartController(ApplicationDBContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration; // Initialize _configuration
      _conn = _configuration.GetConnectionString("DefaultConnection"); // Corrected typo: DefaultConnection
    }

    [HttpPost("Add_to_Cart")]

    public GR<bool> Add_To_Cart(ItemsFromCustomer _items)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          // Insert into _user_detail and _user_other_info
          string query2 = @"INSERT INTO cart (Username, Password)
                              VALUES (@user_name, @password);

                              SELECT SCOPE_IDENTITY();"; // Get the UID of the new user

          int userId = 0;
          using (SqlCommand cmd2 = new SqlCommand(query2, connect))
          {
            cmd2.Parameters.AddWithValue("@user_name", user.Username);
            cmd2.Parameters.AddWithValue("@password", user.Password);

            // Execute the INSERT and get the new user ID
            userId = Convert.ToInt32(cmd2.ExecuteScalar());
          }
          return new GR<bool>
          {
            Success = true,
            Object = true,
            Msg = "User profile created successfully"
          };
        }
      }
      catch (Exception err)
      {
        return new GR<bool>
        {
          Success = false,
          Msg = err.Message
        };
      }
    }
  }
}

// ### 1. `Cart.cs`
// - **AddItemToCart(Item item, int quantity)**: Adds an item to the cart.
// - **RemoveItemFromCart(Item item)**: Removes a specific item from the cart.
// - **UpdateItemQuantity(Item item, int quantity)**: Updates the quantity of a specific item in the cart.
// - **GetCartTotal()**: Calculates and returns the total cost of items in the cart.
// - **ClearCart()**: Clears all items in the cart.
// - **GetItemsInCart()**: Returns a list of all items currently in the cart.
