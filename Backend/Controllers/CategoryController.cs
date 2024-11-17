using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {

    }
}




// ### 2. `Category.cs`
// - **AddCategory(string name, string description)**: Adds a new product category.
// - **RemoveCategory(int categoryId)**: Removes a category by its ID.
// - **UpdateCategory(int categoryId, string newName, string newDescription)**: Updates details of an existing category.
// - **GetCategoryById(int categoryId)**: Retrieves a category by its ID.
// - **GetAllCategories()**: Returns a list of all product categories.
