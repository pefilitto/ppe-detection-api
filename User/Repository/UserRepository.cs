using MySql.Data.MySqlClient;
using ppe_detection_api.Context;
using ppe_detection_api.dto;

namespace ppe_detection_api.User.Repository;

public class UserRepository
{
    private readonly DbContext _context;
    
    public UserRepository(DbContext context)
    {
        _context = context;
    }

    public void Register(dto.User user)
    {
        try
        {
            var conn = _context.CreateConnection();
            
            conn.Open();
            
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO users (Id, UserName, Name, Email, Password) VALUES (@Id, @UserName, @Name, @Email, @Password)";
                cmd.Parameters.Add(new MySqlParameter("@Id", Guid.NewGuid()));
                cmd.Parameters.Add(new MySqlParameter("@UserName", user.UserName));
                cmd.Parameters.Add(new MySqlParameter("@Name", user.Name));
                cmd.Parameters.Add(new MySqlParameter("@Email", user.Email));
                cmd.Parameters.Add(new MySqlParameter("@Password", user.Password));

                cmd.ExecuteNonQuery();
            }
            
            conn.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while registering the user.", ex);
        }
    }

    public dto.User GetUserById(Guid id)
    {
        try
        {
            dto.User user = new dto.User();
            
            var conn = _context.CreateConnection();
            conn.Open();
            
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM users WHERE Id = @Id";
                cmd.Parameters.Add(new MySqlParameter("@Id", id));
                
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user.Id = reader.GetGuid(0);
                        user.UserName = reader["UserName"].ToString();
                        user.Name = reader["Name"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.Password = reader["Password"].ToString();
                    }
                }
            }
            
            conn.Close();

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the user.", ex);
        }
    }
}