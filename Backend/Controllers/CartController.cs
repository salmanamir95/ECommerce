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
        if (_items == null)
        {
          return new GR<bool>
          {
            Success = false,
            Msg = "Invalid input data."
          };
        }

        using (SqlConnection connect = new SqlConnection(_conn))
        {
          // Ensure the connection is opened
          connect.Open();

          string query2 = @"INSERT INTO CART (UID, CID, IID, IQuantity, CreatedAt)
                              VALUES (@UID, @CID, @IID, @IQ, @CreatedAt);";

          using (SqlCommand cmd2 = new SqlCommand(query2, connect))
          {
            // Adding parameters for SQL command
            cmd2.Parameters.AddWithValue("@UID", _items.Userid);
            cmd2.Parameters.AddWithValue("@CID", _items.cartId);
            cmd2.Parameters.AddWithValue("@IID", _items.Itemid);
            cmd2.Parameters.AddWithValue("@IQ", _items.quantity);
            cmd2.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

            // Execute the command
            int rows = cmd2.ExecuteNonQuery();

            // Check if the operation was successful
            if (rows > 0)
            {
              return new GR<bool>
              {
                Success = true,
                Object = true,
                Msg = "Item added to cart successfully."
              };
            }
            else
            {
              return new GR<bool>
              {
                Success = false,
                Msg = "Failed to add item to cart. No rows affected."
              };
            }
          }
        }
      }
      catch (SqlException sqlEx)
      {
        // Handle SQL-specific errors
        return new GR<bool>
        {
          Success = false,
          Msg = "Database error: " + sqlEx.Message
        };
      }
      catch (Exception ex)
      {
        // General exception handling
        return new GR<bool>
        {
          Success = false,
          Msg = "Error: " + ex.Message
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
