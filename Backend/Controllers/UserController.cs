using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.ForJSON;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class UserController : ControllerBase
  {
    private readonly ApplicationDBContext _context;
    private readonly string? _conn;
    private readonly IConfiguration _configuration;  // Declare IConfiguration

    // Inject IConfiguration into the constructor
    public UserController(ApplicationDBContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration; // Initialize _configuration
      _conn = _configuration.GetConnectionString("DefaultConnection"); // Corrected typo: DefaultConnection
    }

    [HttpGet("AllUsers")]

    public GR<List<User_detail>> GetAllUsers()
    {
      try
      {
        List<User_detail> users = new List<User_detail>();
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();
          string query = "SELECT * FROM _user_detail AS U JOIN _user_other_info AS I ON U.UID = I.UID;";
          using (SqlCommand command = new SqlCommand(query, connect))
          {
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {

                User_detail user = new User_detail
                {
                  Uid = Convert.ToInt32(reader["Uid"]),
                  Username = Convert.ToString(reader["Username"]),
                  Password = Convert.ToString(reader["Password"]),
                  UserOtherInfo = new User_Other_Info
                  {
                    Uid = Convert.ToInt32(reader["Uid"]),
                    Email = Convert.ToString(reader["Email"]),
                    PhoneNumber = Convert.ToString(reader["PhoneNumber"])
                  }
                };
                users.Add(user);
              }
            }
            connect.Close();
          }
        }
        return new GR<List<User_detail>>
        {
          Success = true,
          Object = users,
          Msg = "all ok"
        };
      }
      catch (Exception error)
      {
        return new GR<List<User_detail>>
        {
          Success = false,
          Msg = error.Message
        };
      }
    }


    [HttpPost("Login")]
    public GR<bool> login(Login log)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {

          string query = @"
            SELECT *
            FROM _user_detail AS U
            JOIN _user_other_info AS I ON U.UID = I.UID
            WHERE U.username = @Username AND U.password = @Password;
        ";
          using (SqlCommand command = new SqlCommand(query, connect))
          {
            command.Parameters.AddWithValue("@Username", log.username);
            command.Parameters.AddWithValue("@Password", log.password);

            connect.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                if (reader.HasRows)
                {
                  return new GR<bool>
                  {
                    Success = true,
                    Object = true,
                    Msg = "Found"
                  };
                }
                else
                {

                }
              }
            }
            connect.Close();
          }
          return new GR<bool>
          {
            Success = true,
            Object = false,
            Msg = "Not Found"
          };

        }
      }
      catch (Exception error)
      {
        return new GR<bool>
        {
          Success = false,
          Msg = error.Message
        };
      }
    }
    [HttpPost("CreateUserProfile")]

    public GR<bool> CreateUserProfile(SignUp user)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();

          // Check if the username already exists
          string query1 = @"SELECT *
                              FROM _user_detail AS U
                              JOIN _user_other_info AS I ON U.UID = I.UID
                              WHERE U.username = @Username";


          using (SqlCommand command = new SqlCommand(query1, connect))
          {
            command.Parameters.AddWithValue("@Username", user.Username);
            using (SqlDataReader reader = command.ExecuteReader())
            {
              if (reader.HasRows)
              {
                return new GR<bool>
                {
                  Success = false,
                  Object = false,
                  Msg = "Username already exists"
                };
              }
            }
          }

          // Insert into _user_detail and _user_other_info
          string query2 = @"INSERT INTO _user_detail (Username, Password)
                              VALUES (@user_name, @password);

                              SELECT SCOPE_IDENTITY();"; // Get the UID of the new user

          int userId = 0;
          using (SqlCommand cmd2 = new SqlCommand(query2, connect))
          {
            cmd2.Parameters.AddWithValue("@user_name", user.Username);
            cmd2.Parameters.AddWithValue("@password", user.Password);

            // Execute the INSERT and get the new user ID
            userId = Convert.ToInt32(cmd2.ExecuteScalar());
          }

          // Insert into _user_other_info
          string query3 = @"INSERT INTO _user_other_info (UID, Email, PhoneNumber)
                              VALUES (@UID, @Email, @PhoneNumber)";

          using (SqlCommand cmd3 = new SqlCommand(query3, connect))
          {
            cmd3.Parameters.AddWithValue("@UID", userId);
            cmd3.Parameters.AddWithValue("@Email", user.Email);
            cmd3.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);

            cmd3.ExecuteNonQuery(); // Execute the insert for other info
          }

          return new GR<bool>
          {
            Success = true,
            Object = true,
            Msg = "User profile created successfully"
          };
        }
      }
      catch (Exception ex)
      {
        // Log the exception if necessary
        return new GR<bool>
        {
          Success = false,
          Object = false,
          Msg = "An error occurred: " + ex.Message
        };
      }
    }

    //EDITING IN THE END

    [HttpDelete("DeleteUser")]

    public GR<bool> DeleteUserProfile(Login user)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();

          // Corrected query with a proper WHERE clause
          string query1 = "DELETE FROM _user_detail WHERE username = @Username";

          using (SqlCommand command = new SqlCommand(query1, connect))
          {
            // Add the parameter for username
            command.Parameters.AddWithValue("@Username", user.username);

            // Execute the DELETE command
            int rowsAffected = command.ExecuteNonQuery();

            // Optionally check if any rows were deleted
            if (rowsAffected == 0)
            {
              return new GR<bool>
              {
                Success = false,
                Msg = "No user found with the specified username."
              };
            }
          }

          connect.Close();
        }

        return new GR<bool>
        {
          Success = true
        };
      }
      catch (Exception Ex)
      {
        return new GR<bool>
        {
          Success = false,
          Msg = "Error= " + Ex.Message
        };
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
