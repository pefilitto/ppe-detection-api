using Microsoft.AspNetCore.Identity.Data;
using ppe_detection_api.Context;
using ppe_detection_api.Login.Dto;

namespace ppe_detection_api.Login.Repository;

public class LoginRepository
{
    private readonly DbContext _dbContext;
    
    public LoginRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public LoginRequestDto Login(LoginRequestDto request)
    {
        LoginRequestDto response = new LoginRequestDto();
        using var connection = _dbContext.CreateConnection();
        connection.Open();
        
        using(var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT * FROM users WHERE UserName = @UserName";
            command.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@UserName", request.UserName));

            using(var reader = command.ExecuteReader())
            {
                if(reader.Read())
                {
                    response.UserName = reader["UserName"].ToString();
                    response.Password = reader["Password"].ToString();
                }
            }
        }

        connection.Close();

        return response;
    }
}