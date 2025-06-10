public interface IUserRepository
{
    Task<List<UserInfoDTO>> GetUserInfoAsync(string? name, string? family);
}
