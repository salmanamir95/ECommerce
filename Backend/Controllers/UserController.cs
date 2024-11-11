using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
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
          }
          connect.Close();
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
    public GR<bool> login( string username, string password)
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
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

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
