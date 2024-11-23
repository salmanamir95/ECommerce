using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.ForJSON;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class UserController : ControllerBase
  {
    private readonly ApplicationDBContext _context;
    private readonly string? _conn;
    private readonly IConfiguration _configuration;  // Declare IConfiguration

    public UserController(ApplicationDBContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
      _conn = _configuration.GetConnectionString("DefaultConnection");
    }

    [HttpGet("AllUsers")]
    public GR<List<User_detail>> GetAllUsers()
    {
      try
      {
        List<User_detail> users = new List<User_detail>();

        // Using 'using' to ensure connection is disposed properly.
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();
          string query = "SELECT * FROM User_detail AS U JOIN User_Other_Info AS I ON U.UID = I.UID;";
          using (SqlCommand command = new SqlCommand(query, connect))
          {
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                var user = new User_detail
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
        }

        return new GR<List<User_detail>> { Success = true, Object = users, Msg = "all ok" };
      }
      catch (Exception error)
      {
        return new GR<List<User_detail>> { Success = false, Msg = error.Message };
      }
    }

    [HttpPost("Login")]
    public GR<bool> Login(Login log)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          string query = @"
            SELECT *
            FROM User_detail AS U
            JOIN User_other_info AS I ON U.UID = I.UID
            WHERE U.username = @Username AND U.password = @Password;
          ";
          using (SqlCommand command = new SqlCommand(query, connect))
          {
            command.Parameters.AddWithValue("@Username", log.username);
            command.Parameters.AddWithValue("@Password", log.password);

            connect.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
              if (reader.HasRows)
              {
                return new GR<bool> { Success = true, Object = true, Msg = "Login Successful" };
              }
            }

            return new GR<bool> { Success = false, Object = false, Msg = "Invalid username or password" };
          }
        }
      }
      catch (Exception error)
      {
        return new GR<bool> { Success = false, Msg = error.Message };
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
          string query1 = @"SELECT * FROM User_detail AS U WHERE U.username = @Username";
          using (SqlCommand command = new SqlCommand(query1, connect))
          {
            command.Parameters.AddWithValue("@Username", user.Username);
            using (SqlDataReader reader = command.ExecuteReader())
            {
              if (reader.HasRows)
              {
                return new GR<bool> { Success = false, Object = false, Msg = "Username already exists" };
              }
            }
          }

          // Insert new user and retrieve user ID
          string query2 = @"INSERT INTO User_detail (Username, Password) VALUES (@user_name, @password); SELECT SCOPE_IDENTITY();";
          int userId;
          using (SqlCommand cmd2 = new SqlCommand(query2, connect))
          {
            cmd2.Parameters.AddWithValue("@user_name", user.Username);
            cmd2.Parameters.AddWithValue("@password", user.Password);
            userId = Convert.ToInt32(cmd2.ExecuteScalar());
          }

          // Insert additional user information
          string query3 = @"INSERT INTO user_other_info (UID, Email, PhoneNumber) VALUES (@UID, @Email, @PhoneNumber)";
          using (SqlCommand cmd3 = new SqlCommand(query3, connect))
          {
            cmd3.Parameters.AddWithValue("@UID", userId);
            cmd3.Parameters.AddWithValue("@Email", user.Email);
            cmd3.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd3.ExecuteNonQuery();
          }

          return new GR<bool> { Success = true, Object = true, Msg = "User profile created successfully" };
        }
      }
      catch (Exception ex)
      {
        return new GR<bool> { Success = false, Object = false, Msg = "An error occurred: " + ex.Message };
      }
    }

    [HttpDelete("DeleteUser")]
    public GR<bool> DeleteUserProfile(Login user)
    {
      try
      {
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();
          string query = "DELETE FROM user_detail WHERE username = @Username";
          using (SqlCommand command = new SqlCommand(query, connect))
          {
            command.Parameters.AddWithValue("@Username", user.username);
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
              return new GR<bool> { Success = false, Msg = "No user found with the specified username." };
            }
          }

          return new GR<bool> { Success = true, Msg = "User profile deleted successfully" };
        }
      }
      catch (Exception ex)
      {
        return new GR<bool> { Success = false, Msg = "Error: " + ex.Message };
      }
    }
  }
}
