using Domain.Entities;
using Moq;
using Xunit;

 
public class UserProfileManagementTests
{
    private readonly Mock<ICacheService> _mockCache;
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly IUserProfileManagement _userProfileManagement;

    public UserProfileManagementTests()
    {
        _mockCache = new Mock<ICacheService>();
        _mockRepository = new Mock<IUserRepository>();
        _userProfileManagement = new UserProfileManagement(_mockCache.Object, _mockRepository.Object);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsFromCache_WhenExists()
    {
        // Arrange
        var name = "Ali";
        var family = "Rezaei";
        var cachedUsers = new List<UserInfo> { new UserInfo { Id = 1, Name = "Ali", Family = "Rezaei" } };
        _mockCache.Setup(x => x.GetAsync<List<UserInfo>>(It.IsAny<string>()))
                  .ReturnsAsync(cachedUsers);

        // Act
        var result = await _userProfileManagement.GetUserAsync(name, family);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Ali", result[0].Name);
        _mockRepository.Verify(x => x.GetUserInfoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsFromRepositoryAndSetsCache_WhenCacheMiss()
    {
        // Arrange
        var name = "Sara";
        var family = "Ahmadi";
        _mockCache.Setup(x => x.GetAsync<List<UserInfo>>(It.IsAny<string>()))
                  .ReturnsAsync((List<UserInfo>?)null);

        var userDTOs = new List<UserInfoDTO>
        {
            new UserInfoDTO
            {
                Id = 2,
                Name = "Sara",
                Family = "Ahmadi",
                Email = "sara@example.com",
                Certificates = new List<CertificateDTO>
                {
                    new CertificateDTO { Title = "Cert A", Institute = "Inst X", IssuedTime = DateTime.Now }
                },
                WorkHistories = new List<WorkHistoryDTO>
                {
                    new WorkHistoryDTO
                    {
                        Id = 1,
                        CompanyName = "Company A",
                        StartDate = DateTime.Now.AddYears(-2),
                        EndDate = DateTime.Now,
                        Description = "Worked as dev"
                    }
                }
            }
        };

        _mockRepository.Setup(x => x.GetUserInfoAsync(name, family))
                       .ReturnsAsync(userDTOs);

        _mockCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<List<UserInfo>>(), It.IsAny<TimeSpan>()))
                  .Returns(Task.CompletedTask);

        // Act
        var result = await _userProfileManagement.GetUserAsync(name, family);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Sara", result[0].Name);
        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<List<UserInfo>>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsNull_WhenNoData()
    {
        // Arrange
        _mockCache.Setup(x => x.GetAsync<List<UserInfo>>(It.IsAny<string>()))
                  .ReturnsAsync((List<UserInfo>?)null);

        _mockRepository.Setup(x => x.GetUserInfoAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync(new List<UserInfoDTO>());

        // Act
        var result = await _userProfileManagement.GetUserAsync("Unknown", "User");

        // Assert
        Assert.Null(result);
    }
}
