using ppe_detection_api.Common;
using ppe_detection_api.dto;

namespace ppe_detection_api.Role.Dto;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public HelmetColor HelmetColor { get; set; }
    
    //public List<PPE> RequiredPPEs { get; set; }
}