using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Backend.Data;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CategoryController : ControllerBase
  {
    private readonly ApplicationDBContext _context;
    private readonly string? _conn;
    private readonly IConfiguration _configuration;  // Declare IConfiguration

    public CategoryController(ApplicationDBContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
      _conn = _configuration.GetConnectionString("DefaultConnection");
    }

    // Add a new Category (Asynchronous)
    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory([FromBody] Category category)
    {
      if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
      {
        return BadRequest(new { message = "Category name is required." });
      }

      try
      {
        using (SqlConnection connection = new SqlConnection(_conn))
        {
          await connection.OpenAsync();
          string query = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";

          using (SqlCommand command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            await command.ExecuteNonQueryAsync();
          }

          return Ok(new { message = "Category added successfully." });
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = ex.Message });
      }
    }

    // Get a category by ID (Asynchronous)
    [HttpGet("GetCategoryById/{categoryId}")]
    public async Task<IActionResult> GetCategoryById(int categoryId)
    {
      try
      {
        using (SqlConnection connection = new SqlConnection(_conn))
        {
          await connection.OpenAsync();
          string query = "SELECT * FROM Categories WHERE Cid = @Cid";

          using (SqlCommand command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Cid", categoryId);
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
              if (await reader.ReadAsync())
              {
                var category = new Category
                {
                  Cid = (int)reader["Cid"],
                  CategoryName = (string)reader["CategoryName"]
                };
                return Ok(category);
              }
              else
              {
                return NotFound(new { message = "Category not found." });
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = ex.Message });
      }
    }

    // Update a Category (Asynchronous)
    [HttpPut("UpdateCategory/{categoryId}")]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] Category category)
    {
      if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
      {
        return BadRequest(new { message = "Category name is required." });
      }

      try
      {
        using (SqlConnection connection = new SqlConnection(_conn))
        {
          await connection.OpenAsync();
          string query = "UPDATE Categories SET CategoryName = @CategoryName WHERE Cid = @Cid";

          using (SqlCommand command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            command.Parameters.AddWithValue("@Cid", categoryId);
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
              return Ok(new { message = "Category updated successfully." });
            }
            else
            {
              return NotFound(new { message = "Category not found." });
            }
          }
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = ex.Message });
      }
    }

    // Remove a Category (Asynchronous)
    [HttpDelete("RemoveCategory/{categoryId}")]
    public async Task<IActionResult> RemoveCategory(int categoryId)
    {
      try
      {
        using (SqlConnection connection = new SqlConnection(_conn))
        {
          await connection.OpenAsync();
          string query = "DELETE FROM Categories WHERE Cid = @Cid";

          using (SqlCommand command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Cid", categoryId);
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
              return Ok(new { message = "Category deleted successfully." });
            }
            else
            {
              return NotFound(new { message = "Category not found." });
            }
          }
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = ex.Message });
      }
    }

    // Get all Categories (Asynchronous)
    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
      try
      {
        List<Category> categories = new List<Category>();

        using (SqlConnection connection = new SqlConnection(_conn))
        {
          await connection.OpenAsync();
          string query = "SELECT * FROM Categories";

          using (SqlCommand command = new SqlCommand(query, connection))
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var category = new Category
              {
                Cid = (int)reader["Cid"],
                CategoryName = (string)reader["CategoryName"]
              };
              categories.Add(category);
            }
          }
        }

        return Ok(categories);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = ex.Message });
      }
    }
  }
}


// ### 2. `Category.cs`
// - **AddCategory(string name, string description)**: Adds a new product category.
// - **RemoveCategory(int categoryId)**: Removes a category by its ID.
// - **UpdateCategory(int categoryId, string newName, string newDescription)**: Updates details of an existing category.
// - **GetCategoryById(int categoryId)**: Retrieves a category by its ID.
// - **GetAllCategories()**: Returns a list of all product categories.
