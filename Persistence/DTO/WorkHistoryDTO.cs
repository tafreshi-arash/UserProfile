﻿
public class WorkHistoryDTO
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }


    public int UserId { get; set; }
    public UserInfoDTO UserInfo { get; set; }
}

