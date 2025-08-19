using MySql.Data.MySqlClient;
using ppe_detection_api.Context;

namespace ppe_detection_api.PPE.Repository;

public class PPERepository
{
    private readonly DbContext _context;
    
    public PPERepository(DbContext context)
    {
        _context = context;
    }
    
    public void Register(dto.PPE ppe)
    {
        var connection = _context.CreateConnection();
        
        connection.Open();
        
        using(var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO ppe (id, name) VALUES (@id, @name)";
            command.Parameters.Add(new MySqlParameter("@id", ppe.Id));
            command.Parameters.Add(new MySqlParameter("@name", ppe.Name));
            
            command.ExecuteNonQuery();
        }
        
        connection.Close();
    }
    
    public List<dto.PPE> GetAll()
    {
        var connection = _context.CreateConnection();
        var ppes = new List<dto.PPE>();
        
        connection.Open();
        
        using(var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT id, name FROM ppe";
            
            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    var ppe = new dto.PPE
                    {
                        Id = reader.GetGuid(0),
                        Name = reader["name"].ToString()
                    };
                    
                    ppes.Add(ppe);
                }
            }
        }
        
        connection.Close();
        
        return ppes;
    }
    
    public List<dto.PPE> GetPPEsByRoleId(Guid roleId)
    {
        var connection = _context.CreateConnection();
        List<dto.PPE> ppeList = new List<dto.PPE>();
        
        connection.Open();
        
        using(var command = connection.CreateCommand())
        {
            command.CommandText = @"select p.id as ppe_id, p.name as ppe_name from role_required_ppes rrp
                                    inner join ppe p on p.id = rrp.ppe_id
                                    where rrp.role_id = @id";
            command.Parameters.Add(new MySqlParameter("@id", roleId.ToString()));
            
            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    dto.PPE ppe = new dto.PPE();
                    ppe.Id = reader.GetGuid(0);
                    ppe.Name = reader["ppe_name"].ToString();
                    
                    ppeList.Add(ppe);
                }
            }
        }
        
        connection.Close();
        
        return ppeList;
    }
}