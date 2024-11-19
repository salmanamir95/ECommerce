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

            foreach (var item in total.IID)
            {
                string query = @"SELECT CART.IQuantity, ITEM.Quantity, ITEM.Name
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

                            if (cartQuantity > stockQuantity)
                            {
                                insufficientItems.Add($"{itemName} (Requested: {cartQuantity}, Available: {stockQuantity})");
                            }
                            else
                            {
                                cartItemsQuantities[item] = cartQuantity;
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
            using (SqlTransaction transaction = connect.BeginTransaction())
            {
                try
                {
                    foreach (var cartItem in cartItemsQuantities)
                    {
                        string updateStockQuery = @"UPDATE ITEM
                                                    SET Quantity = Quantity - @Quantity
                                                    WHERE ITEMID = @IID";
                        using (SqlCommand updateCmd = new SqlCommand(updateStockQuery, connect, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@Quantity", cartItem.Value);
                            updateCmd.Parameters.AddWithValue("@IID", cartItem.Key);
                            updateCmd.ExecuteNonQuery();
                        }
                    }

                    // Assuming Receipt table exists for storing receipt details
                    // string insertReceiptQuery = @"INSERT INTO (UID, CID, CreatedAt, PaymentMethod, Description)
                    //                               OUTPUT INSERTED.ReceiptID
                    //                               VALUES (@UID, @CID, @CreatedAt, @PaymentMethod, @Desc)";
                    //int receiptId;
                    // using (SqlCommand receiptCmd = new SqlCommand(insertReceiptQuery, connect, transaction))
                    // {
                    //     receiptCmd.Parameters.AddWithValue("@UID", total.Uid);
                    //     receiptCmd.Parameters.AddWithValue("@CID", total.CartId);
                    //     receiptCmd.Parameters.AddWithValue("@CreatedAt", total.CreatedAt);
                    //     receiptCmd.Parameters.AddWithValue("@PaymentMethod", total.PaymentMethod);
                    //     receiptCmd.Parameters.AddWithValue("@Desc", total.Desc ?? string.Empty);
                    //     receiptId = (int)receiptCmd.ExecuteScalar();
                    // }

                    // transaction.Commit();

                    // // Return the receipt details
                    return new GR<Receipt>
                    {
                        Success = true,
                        Object = new Receipt
                        {
                            UserId = total.Uid,
                            CartId = total.CartId,
                            CreatedAt = total.CreatedAt,
                            PaymentMethod = total.PaymentMethod,
                            Description = total.Desc
                        }
                    };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
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


  }
}

// ### 5. `Order.cs`
// - **CreateOrder(User_detail user, Cart cart, string paymentMethod)**: Creates a new order.
// - **GetOrderDetails(int orderId)**: Retrieves the details of a specific order.
// - **UpdateOrderStatus(int orderId, string newStatus)**: Updates the status of an order (e.g., from "Processing" to "Shipped").
// - **CancelOrder(int orderId)**: Cancels an existing order.
// - **GetUserOrderHistory(int userId)**: Returns a list of all orders made by a specific user.

