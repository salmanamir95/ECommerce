using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        List<User_detail> users= new List<User_detail>();
        using (SqlConnection connect = new SqlConnection(_conn))
        {
          connect.Open();
          string query = "SELECT * FROM _user_detail AS U JOIN _user_other_info AS I ON U.UID = I.UID;";
          using (SqlCommand command = new SqlCommand(query, connect))
          {
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while(reader.Read())
              {
                User_detail user = new User_detail{
                Uid= Convert.ToInt32(reader["Uid"]),
                Username= Convert.ToString(reader["Username"]),
                Password= Convert.ToString(reader["Password"]),
                UserOtherInfo= new User_Other_Info{
                  Uid= Convert.ToInt32(reader["Uid"]),
                  Email= Convert.ToString(reader["Email"]),
                  PhoneNumber= Convert.ToString(reader["PhoneNumber"])
                }
                };
                users.Add(user);
              }
            }
          }
        }
        return new GR<List<User_detail>> {
          Success= true,
          Object= users,
          Msg= "all ok"
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
  }
}
