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
      catch(Exception ex)
      {
        return new GR<List<Order>> {
          Success=false,
          Msg= ex.Message
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

