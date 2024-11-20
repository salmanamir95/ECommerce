using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly string _conn;

        public CategoryController(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DefaultConnection"); // Assuming you have a DefaultConnection in your appsettings.json
        }

        // 1. AddCategory: Adds a new product category.
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is required.");
            }

            using (SqlConnection connect = new SqlConnection(_conn))
            {
                try
                {
                    await connect.OpenAsync();

                    string query = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName);";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = category.Cid }, category);
                        }
                        else
                        {
                            return StatusCode(500, "Failed to add category.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        // 2. RemoveCategory: Removes a category by its ID.
        [HttpDelete("RemoveCategory/{categoryId}")]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            using (SqlConnection connect = new SqlConnection(_conn))
            {
                try
                {
                    await connect.OpenAsync();

                    string query = "DELETE FROM Categories WHERE Cid = @CategoryId;";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return Ok($"Category with ID {categoryId} has been deleted.");
                        }
                        else
                        {
                            return NotFound($"Category with ID {categoryId} not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        // 3. UpdateCategory: Updates an existing category.
        [HttpPut("UpdateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] Category updatedCategory)
        {
            if (updatedCategory == null || categoryId != updatedCategory.Cid)
            {
                return BadRequest("Category data is invalid.");
            }

            using (SqlConnection connect = new SqlConnection(_conn))
            {
                try
                {
                    await connect.OpenAsync();

                    string query = "UPDATE Categories SET CategoryName = @CategoryName WHERE Cid = @CategoryId;";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmd.Parameters.AddWithValue("@CategoryName", updatedCategory.CategoryName);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return NoContent(); // No content indicates a successful update
                        }
                        else
                        {
                            return NotFound($"Category with ID {categoryId} not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        // 4. GetCategoryById: Retrieves a category by its ID.
        [HttpGet("GetCategoryById/{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            using (SqlConnection connect = new SqlConnection(_conn))
            {
                try
                {
                    await connect.OpenAsync();

                    string query = "SELECT * FROM Categories WHERE Cid = @CategoryId;";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var category = new Category
                                {
                                    Cid = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1)
                                };

                                return Ok(category);
                            }
                            else
                            {
                                return NotFound($"Category with ID {categoryId} not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        // 5. GetAllCategories: Returns a list of all product categories.
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            using (SqlConnection connect = new SqlConnection(_conn))
            {
                try
                {
                    await connect.OpenAsync();

                    string query = "SELECT * FROM Categories;";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            var categories = new List<Category>();

                            while (await reader.ReadAsync())
                            {
                                var category = new Category
                                {
                                    Cid = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1)
                                };

                                categories.Add(category);
                            }

                            return Ok(categories);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}


// ### 6. `User_detail.cs`
// - **CreateUserProfile(string name, string email, string password)**: Adds a new user profile.
// - **UpdateUserProfile(int userId, string newName, string newEmail)**: Updates user details.
// - **GetUserProfile(int userId)**: Retrieves the profile information of a user.
// - **AuthenticateUser(string email, string password)**: Checks if user credentials are valid.
// - **DeleteUserProfile(int userId)**: Deletes a user profile.

// ### 7. `User_Other_Info.cs`
// - **AddUserOtherInfo(int userId, string address, string phoneNumber)**: Adds additional information for a user.
// - **UpdateUserOtherInfo(int userId, string newAddress, string newPhoneNumber)**: Updates additional information for a user.
// - **GetUserOtherInfo(int userId)**: Retrieves other information related to a user.
// - **DeleteUserOtherInfo(int userId)**: Deletes additional information related to a user.
