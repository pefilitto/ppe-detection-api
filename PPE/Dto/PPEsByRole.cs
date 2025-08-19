using ppe_detection_api.Common;

namespace ppe_detection_api.dto;

public class PPEsByRole
{
    public Role.Dto.Role Role { get; set; }
    public List<PPE> RequiredPPEs { get; set; } 
}