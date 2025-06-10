using Domain.Entities;


public interface IUserProfileManagement
{
    Task<List<UserInfo>> GetUserAsync(string? name, string? family);
}

