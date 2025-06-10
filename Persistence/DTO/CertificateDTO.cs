
public class CertificateDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Institute { get; set; }
    public DateTime IssuedTime { get; set; }


    public int UserId { get; set; }
    public UserInfoDTO UserInfo { get; set; }
}

