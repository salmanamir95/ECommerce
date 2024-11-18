using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

    }
}

// ### 5. `Order.cs`
// - **CreateOrder(User_detail user, Cart cart, string paymentMethod)**: Creates a new order.
// - **GetOrderDetails(int orderId)**: Retrieves the details of a specific order.
// - **UpdateOrderStatus(int orderId, string newStatus)**: Updates the status of an order (e.g., from "Processing" to "Shipped").
// - **CancelOrder(int orderId)**: Cancels an existing order.
// - **GetUserOrderHistory(int userId)**: Returns a list of all orders made by a specific user.

