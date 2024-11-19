using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.ForJSON;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

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
              connect.Close();
              return new GR<bool>
              {
                Success = true,
                Object = true,
                Msg = "Item added to cart successfully."
              };
            }
            else
            {
              connect.Close();
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

    [HttpPost("Remove_From_Cart")]
    public GR<bool> Remove_From_Cart(CartUserInfo _items)
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

          string query2 = @"DELETE FROM CART
                              WHERE cart.UID= @UID AND CART.CID = @CID AND CART.IID= @IID";

          using (SqlCommand cmd2 = new SqlCommand(query2, connect))
          {
            // Adding parameters for SQL command
            cmd2.Parameters.AddWithValue("@UID", _items.Userid);
            cmd2.Parameters.AddWithValue("@CID", _items.cartId);
            cmd2.Parameters.AddWithValue("@IID", _items.Itemid);

            // Execute the command
            int rows = cmd2.ExecuteNonQuery();

            // Check if the operation was successful
            if (rows > 0)
            {
              connect.Close();
              return new GR<bool>
              {
                Success = true,
                Object = true,
                Msg = "Item deleted from cart successfully."
              };
            }
            else
            {
              connect.Close();
              return new GR<bool>
              {
                Success = false,
                Msg = "Failed to delete item from cart. No rows affected."
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

    [HttpPost("Total_Cart")]
    public GR<int> Total_Cart(CartTotal _cart)
    {
      try
      {
        // Validate input data
        if (_cart == null || _cart.Userid <= 0 || _cart.cartId <= 0)
        {
          return new GR<int>
          {
            Success = false,
            Msg = "Invalid input data."
          };
        }

        // Open SQL connection
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();

          // SQL query to count the number of items in the cart
          string query = @"SELECT COUNT(*)
                             FROM CART
                             WHERE UID = @UID AND CID = @CID";

          // Create SQL command
          using (SqlCommand cmd = new SqlCommand(query, connect))
          {
            // Add parameters to prevent SQL injection
            cmd.Parameters.Add("@UID", SqlDbType.Int).Value = _cart.Userid;
            cmd.Parameters.Add("@CID", SqlDbType.Int).Value = _cart.cartId;

            // Execute the query and get the count of items
            int itemCount = (int)cmd.ExecuteScalar();

            // Return the item count
            return new GR<int>
            {
              Success = true,
              Object = itemCount,
              Msg = "Total items in the cart retrieved successfully."
            };
          }
        }
      }

      catch (Exception ex)
      {

        return new GR<int>
        {
          Success = false,
          Msg = "Error: " + ex.Message
        };
      }
    }

    [HttpPost("GetCart")]

    public GR<List<ReturnQuantityInfoPerItem>> Get_Cart(CartTotal _cart)
    {
      try
      {
        List<ReturnQuantityInfoPerItem> MyData= new List<ReturnQuantityInfoPerItem>();
        // Validate input data
        if (_cart == null || _cart.Userid <= 0 || _cart.cartId <= 0)
        {
          return new GR<List<ReturnQuantityInfoPerItem>>
          {
            Success = false,
            Msg = "Invalid input data."
          };
        }

        // Open SQL connection
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();

          // SQL query to count the number of items in the cart
          string query = @"SELECT *
                             FROM CART
                             WHERE UID = @UID AND CID = @CID";

          // Create SQL command
          using (SqlCommand cmd = new SqlCommand(query, connect))
          {
            // Add parameters to prevent SQL injection
            cmd.Parameters.Add("@UID", SqlDbType.Int).Value = _cart.Userid;
            cmd.Parameters.Add("@CID", SqlDbType.Int).Value = _cart.cartId;
            using (SqlDataReader reader= cmd.ExecuteReader())
            {
              while(reader.Read()){
                ReturnQuantityInfoPerItem user_thing = new ReturnQuantityInfoPerItem{
                  UserCartInfo= _cart,
                  itemId= Convert.ToInt32(reader["IID"]),
                  quantity= Convert.ToInt32(reader["IQuantity"])
                };
                MyData.Add(user_thing);
              }

            }


          }
          connect.Close();
        }
        return new GR<List<ReturnQuantityInfoPerItem>>{
          Success=true,
          Object=MyData,

        };
      }

      catch (Exception ex)
      {

        return new GR<List<ReturnQuantityInfoPerItem>>
        {
          Success = false,
          Msg = "Error: " + ex.Message
        };
      }
    }

    [HttpPost("ClearCart")]

    public GR<bool> Clear_Cart(CartTotal user_info)
    {
      using (SqlConnection connect = new SqlConnection(_conn))
        {
          // Ensure the connection is opened
          connect.Open();

          string query2 = @"DELETE FROM CART
                              WHERE cart.UID= @UID AND CART.CID = @CID";

          using (SqlCommand cmd2 = new SqlCommand(query2, connect))
          {
            // Adding parameters for SQL command
            cmd2.Parameters.AddWithValue("@UID", user_info.Userid);
            cmd2.Parameters.AddWithValue("@CID", user_info.cartId);


            // Execute the command
            int rows = cmd2.ExecuteNonQuery();

            // Check if the operation was successful
            if (rows > 0)
            {
              connect.Close();
              return new GR<bool>
              {
                Success = true,
                Object = true,
                Msg = "Item deleted from cart successfully."
              };
            }
            else
            {
              connect.Close();
              return new GR<bool>
              {
                Success = false,
                Msg = "Failed to delete item from cart. No rows affected."
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
