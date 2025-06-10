using Domain.Entities;


public class UserProfileManagement : IUserProfileManagement
{

    private ICacheService _cache;
    private IUserRepository _userRepository;

    public UserProfileManagement(ICacheService cache, IUserRepository userRepository)
    {
        this._cache = cache;
        this._userRepository = userRepository;
    }

    public async Task<List<UserInfo>> GetUserAsync(string? name, string? family)
    {
        try
        {
            string key = GeneratedUserCacheKey(name, family);
            List<UserInfo>? userInfos = await _cache.GetAsync<List<UserInfo>>(key);

            if (userInfos != null)
                return userInfos;

            var result = await _userRepository.GetUserInfoAsync(name, family);
            if (result is null || result.Count() == 0)
                return null;

            List<UserInfo> userInfoes = ConvertToUserInfo(result); // Alternatively, we can Use Mapper
            await _cache.SetAsync(key, userInfoes, TimeSpan.FromMinutes(5));

            return userInfoes;
        }
        catch (Exception ex)
        {
            // Log (SeriLog or Microsoft Extension Log)
            // Temporary using console log
            Console.WriteLine(ex.Message.ToString());
            return null;
        }
    }

    private List<UserInfo> ConvertToUserInfo(List<UserInfoDTO> userDTOList)
    {
        return userDTOList.Select(dto => new UserInfo
        {
            Id = dto.Id,
            Name = dto.Name,
            Family = dto.Family,
            Email = dto.Email,
            Certificates = dto.Certificates.Select(c => new Certificate
            {
                Title = c.Title,
                Institute = c.Institute,
                IssuedTime = DateOnly.FromDateTime(c.IssuedTime)
            }).ToList(),
            WorkHistories = dto.WorkHistories.Select(w => new WorkHistory
            {
                Id = w.Id,
                CompanyName = w.CompanyName,
                StartDate = DateOnly.FromDateTime(w.StartDate),
                EndDate = DateOnly.FromDateTime(w.EndDate),
                Description = w.Description
            }).ToList()
        }).ToList();
    }

    private string GeneratedUserCacheKey(string? name, string? family)
    {
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(family))
            return $"user:name:{name}:family:{family}";
        else if (!string.IsNullOrEmpty(name))
            return $"user:name:{name}";
        else if (!string.IsNullOrEmpty(family))
            return $"user:family:{family}";
        else
            return "user:all";
    }
}

