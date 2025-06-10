
public class UserInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public string Email { get; set; }
    public ICollection<WorkHistoryDTO> WorkHistories { get; set; } = new List<WorkHistoryDTO>();
    public ICollection<CertificateDTO> Certificates { get; set; } = new List<CertificateDTO>();
}

