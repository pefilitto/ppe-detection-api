using System.Configuration;
using MySql.Data.MySqlClient;
using Mysqlx.Session;
using Org.BouncyCastle.Cms;
using ppe_detection_api.Common;
using ppe_detection_api.Context;
using ppe_detection_api.dto;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ppe_detection_api.Role.Repository;

public class RoleRepository
{
    private readonly DbContext _dbContext;
    private MySqlTransaction _transaction;
    private readonly MySqlConnection _connection;
    
    public RoleRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _connection = (MySqlConnection)_dbContext.CreateConnection();
    }
    
    public Guid Register(Dto.Role role)
    {
        Guid roleId = Guid.NewGuid();
        
        using(var command = _connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO role (id, name, helmetcolor) VALUES (@id, @name, @helmetcolor)";
            command.Parameters.Add(new MySqlParameter("@helmetcolor", role.HelmetColor));
            command.Parameters.Add(new MySqlParameter("@id", roleId));
            command.Parameters.Add(new MySqlParameter("@name", role.Name));
            
            command.ExecuteNonQuery();
        }

        return roleId;
    }
    
    public void RegisterRequiredPPEs(Guid roleId, Guid PPEId)
    {
        using(var command = _connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO role_required_ppes (id, role_id, ppe_id) VALUES (@id, @role_id, @ppe_id)";
            command.Parameters.Add(new MySqlParameter("@id", Guid.NewGuid()));
            command.Parameters.Add(new MySqlParameter("@role_id", roleId));
            command.Parameters.Add(new MySqlParameter("@ppe_id", PPEId));
            
            command.ExecuteNonQuery();
        }
    }
    
    public Dto.Role GetRoleById(Guid roleId)
    {
        Dto.Role role = new Dto.Role();
        
        OpenConnection();
        
        using(var command = _connection.CreateCommand())
        {
            command.CommandText = "SELECT id, name, helmetcolor FROM role WHERE id = @id";
            command.Parameters.Add(new MySqlParameter("@id", roleId.ToString()));
            
            using(var reader = command.ExecuteReader())
            {
                if(reader.Read())
                {
                    role.Id = reader.GetGuid(0);
                    role.Name = reader["name"].ToString();
                    role.HelmetColor = (HelmetColor)Enum.Parse(typeof(HelmetColor), reader["helmetcolor"].ToString());
                }
            }
        }
        
        CloseConnection();

        return role;
    }
    
    public void BeginTransaction()
    {
        OpenConnection();
        _transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        _transaction.Commit();
        CloseConnection();
    }

    public void RollBack()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
        }
        
        CloseConnection();
    }

    private void OpenConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            _connection.Open();
        }
    }
    
    private void CloseConnection()
    {
        if (_connection.State == System.Data.ConnectionState.Open)
        {
            _connection.Close();
        }
    }
}