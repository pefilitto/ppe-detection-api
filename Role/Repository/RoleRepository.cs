using ppe_detection_api.Context;

namespace ppe_detection_api.Role.Repository;

public class RoleRepository
{
    private readonly DbContext _dbContext;
    
    public RoleRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void Register(Dto.Role role)
    {
        using var connection = _dbContext.CreateConnection();
        connection.Open();
        
        using(var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO roles (Id, Name) VALUES (@Id, @Name)";
            command.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@Id", role.Id));
            command.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@Name", role.Name));
            
            command.ExecuteNonQuery();
        }
        
        connection.Close();
    }
}