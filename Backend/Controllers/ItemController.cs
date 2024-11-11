using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {

    }
}

// ### 4. `Item.cs`
// - **CreateItem(string name, decimal price, int categoryId, string description)**: Adds a new item to the inventory.
// - **UpdateItem(int itemId, string newName, decimal newPrice, string newDescription)**: Updates details of an existing item.
// - **DeleteItem(int itemId)**: Removes an item from the inventory.
// - **GetItemById(int itemId)**: Retrieves an item by its ID.
// - **SearchItems(string keyword)**: Searches for items based on a keyword and returns a list of matching items.
