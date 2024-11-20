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
  public class OrderController : ControllerBase
  {
    private readonly ApplicationDBContext _context;
    private readonly string? _conn;
    private readonly IConfiguration _configuration;  // Declare IConfiguration

    // Inject IConfiguration into the constructor
    public OrderController(ApplicationDBContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration; // Initialize _configuration
      _conn = _configuration.GetConnectionString("DefaultConnection"); // Corrected typo: DefaultConnection
    }

    [HttpGet("OrderHistory")]

    public GR<List<Order>> GetAll(CartTotal data)
    {
      try
      {
        List<Order> orders = new List<Order>();
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();
          string query = "SELECT * FROM Orders AS O WHERE O.CartUID = @UID AND 0.CARTCID= @CID;";
          using (SqlCommand command = new SqlCommand(query, connect))
          {
            command.Parameters.AddWithValue("@UID", data.Userid);
            command.Parameters.AddWithValue("@CID", data.cartId);
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {

                Order ord = new Order
                {
                  Oid = Convert.ToInt32(reader["Oid"]),
                  Uid = Convert.ToInt32(reader["CartUID"]),
                  CartId = Convert.ToInt32(reader["CartCID"]),
                  Desc = Convert.ToString(reader["Order_Detail"]),
                  CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                  PaymentMethod = Convert.ToChar(reader["PaymentMethod"]),
                  IID = Convert.ToInt32(reader["CartIID"])

                };
                orders.Add(ord);
              }
            }
            connect.Close();
          }

          return new GR<List<Order>>
          {

            Success = true,
            Object = orders
          };
        }
      }
      catch (Exception ex)
      {
        return new GR<List<Order>>
        {
          Success = false,
          Msg = ex.Message
        };
      }
    }

    [HttpPost("Genreate_Receipt")]

    public GR<Receipt> GenerateReceipt(OrderFromCustomer total)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();

          // To track feasibility
          List<string> insufficientItems = new List<string>();
          Dictionary<int, int> cartItemsQuantities = new Dictionary<int, int>(); // Stores itemId and cart quantity
          int total_price = 0;
          foreach (var item in total.IID)
          {
            string query = @"SELECT CART.IQuantity, ITEM.Quantity, ITEM.Name, ITEM.PRICE
                                 FROM ITEM
                                 JOIN CART ON ITEM.ITEMID = CART.IID
                                 WHERE CART.UID = @UID AND CART.IID = @IID AND CART.CID = @CID";
            using (SqlCommand cmd = new SqlCommand(query, connect))
            {
              cmd.Parameters.AddWithValue("@UID", total.Uid);
              cmd.Parameters.AddWithValue("@IID", item);
              cmd.Parameters.AddWithValue("@CID", total.CartId);

              using (SqlDataReader reader = cmd.ExecuteReader())
              {
                if (reader.Read())
                {
                  int cartQuantity = reader.GetInt32(0);
                  int stockQuantity = reader.GetInt32(1);
                  string itemName = reader.GetString(2);
                  int temp_price = reader.GetInt32(3);

                  if (cartQuantity > stockQuantity)
                  {
                    insufficientItems.Add($"{itemName} (Requested: {cartQuantity}, Available: {stockQuantity})");
                  }
                  else
                  {
                    cartItemsQuantities[item] = cartQuantity;
                    total_price += cartQuantity * temp_price;
                  }
                }
                else
                {
                  return new GR<Receipt>
                  {
                    Success = false,
                    Msg = $"Item with ID {item} not found in cart."
                  };
                }
              }
            }
          }

          // Check if any items were insufficient
          if (insufficientItems.Any())
          {
            return new GR<Receipt>
            {
              Success = false,
              Msg = $"Insufficient stock for: {string.Join(", ", insufficientItems)}"
            };
          }

          // Generate receipt (update stock and create receipt entry)
          return new GR<Receipt>
          {
            Success = true,
            Object = new Receipt
            {
              UserId = total.Uid,
              CartId = total.CartId,
              Items = total.IID,
              TotalPrice = total_price,
              PaymentMethod = total.PaymentMethod
            }

          };
        }
      }
      catch (Exception ex)
      {
        return new GR<Receipt>
        {
          Success = false,
          Msg = ex.Message
        };
      }
    }

    [HttpPost("Order_Affirmed")]
    public GR<bool> OrderAffirmed(Receipt total)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();

          using (SqlTransaction transaction = connect.BeginTransaction())
          {
            try
            {
              // Step 1: Update item quantities by subtracting the cart quantities
              string updateItemQuery = @"
                        UPDATE ITEM
                        SET Quantity = ITEM.Quantity - CART.IQuantity
                        FROM ITEM
                        INNER JOIN CART ON ITEM.ITEMID = CART.IID
                        WHERE CART.UID = @UID AND CART.CID = @CID";

              using (SqlCommand cmd = new SqlCommand(updateItemQuery, connect, transaction))
              {
                cmd.Parameters.AddWithValue("@UID", total.UserId);
                cmd.Parameters.AddWithValue("@CID", total.CartId);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                  transaction.Rollback();
                  return new GR<bool>
                  {
                    Success = false,
                    Msg = "No matching cart items found to update."
                  };
                }
              }

              // Step 2: Insert a new order record
              string insertOrderQuery = @"
                        INSERT INTO Orders (CartUID, CartCID, Order_Detail, CreatedAt, PaymentMethod, CartIID)
                        VALUES (@CartUID, @CartCID, @OrderDetail, @CreatedAt, @PaymentMethod, @CartIID);
                        SELECT SCOPE_IDENTITY();"; // Get the generated Order ID

              using (SqlCommand cmd = new SqlCommand(insertOrderQuery, connect, transaction))
              {
                cmd.Parameters.AddWithValue("@CartUID", total.UserId);
                cmd.Parameters.AddWithValue("@CartCID", total.CartId);
                cmd.Parameters.AddWithValue("@OrderDetail", total.Description ?? "No description");
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@PaymentMethod", total.PaymentMethod);
                cmd.Parameters.AddWithValue("@CartIID", string.Join(",", total.Items)); // If you want to store Item IDs

                int orderId = Convert.ToInt32(cmd.ExecuteScalar());

                if (orderId <= 0)
                {
                  transaction.Rollback();
                  return new GR<bool>
                  {
                    Success = false,
                    Msg = "Failed to create the order."
                  };
                }

                // Optionally, you could set this orderId to the Receipt object or send it in the response
                total.Description = "Order created successfully with Order ID " + orderId;
              }

              // Step 3: Commit the transaction if all operations are successful
              transaction.Commit();
              return new GR<bool>
              {
                Success = true,
                Msg = "Order successfully affirmed and stock updated."
              };
            }
            catch (Exception ex)
            {
              // If any error occurs, rollback the transaction
              transaction.Rollback();
              return new GR<bool>
              {
                Success = false,
                Msg = ex.Message
              };
            }
          }
        }
      }
      catch (Exception ex)
      {
        return new GR<bool>
        {
          Success = false,
          Msg = ex.Message
        };
      }
    }



  }
}

// ### 5. `Order.cs`
// - **CreateOrder(User_detail user, Cart cart, string paymentMethod)**: Creates a new order.
// - **GetOrderDetails(int orderId)**: Retrieves the details of a specific order.
// - **UpdateOrderStatus(int orderId, string newStatus)**: Updates the status of an order (e.g., from "Processing" to "Shipped").
// - **CancelOrder(int orderId)**: Cancels an existing order.
// - **GetUserOrderHistory(int userId)**: Returns a list of all orders made by a specific user.

