using Bogus;
using Domain.Entities;
using Moq;


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
        // Arrang
        string name = "علی";

        var cachedUsers = new List<UserInfo> { new UserInfo { Id = 1, Name = "علی", Family = "رضایی" } };

        _mockCache.Setup(x => x.GetAsync<List<UserInfo>>(It.IsAny<string>()))
                  .ReturnsAsync(cachedUsers);

        // Act
        var result = await _userProfileManagement.GetUserAsync(name, null);

        // Assert
        Assert.NotNull(result);

        foreach (var item in result)
            Assert.Equal("علی", item.Name);

        _mockRepository.Verify(x => x.GetUserInfoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsFromRepositoryAndSetsCache_WhenInputExistsInDb_AndCacheMiss()
    {
        // Arrang
        _mockCache.Setup(x => x.GetAsync<List<UserInfo>>(It.IsAny<string>()))
                  .ReturnsAsync((List<UserInfo>?)null);

        var userDTOs = new List<UserInfoDTO>
        {
            new UserInfoDTO
            {
                Id = 1,
                Name = "سارا",
                Family = "احمدی",
                Email = "sara@example.com",
                Certificates = new List<CertificateDTO>
                {
                    new CertificateDTO { Title = "کارشناسی برق", Institute = "دانشگاه خوارزمی", IssuedTime = DateTime.Now }
                },
                WorkHistories = new List<WorkHistoryDTO>
                {
                    new WorkHistoryDTO
                    {
                        Id = 1,
                        CompanyName = "صنایع انفوماتیک",
                        StartDate = DateTime.Now.AddYears(-2),
                        EndDate = DateTime.Now,
                        Description = "کارشناس ارشد طراحی سیستم"
                    }
                }
            },
             new UserInfoDTO
            {
                Id = 2,
                Name = "سارا",
                Family = "محمدی نیا",
                Email = "sara@example.com",
                Certificates = new List<CertificateDTO>
                {
                    new CertificateDTO { Title = "کارشناسی برق", Institute = "دانشگاه علامه طبابایی", IssuedTime = DateTime.Now }
                },
                WorkHistories = new List<WorkHistoryDTO>
                {
                    new WorkHistoryDTO
                    {
                        Id = 1,
                        CompanyName = "مپنا",
                        StartDate = DateTime.Now.AddYears(-2),
                        EndDate = DateTime.Now,
                        Description = "کارشناس برنامه نویسی و توسعه نرم افزار"
                    }
                }
            },
        };

        string name = "سارا";
        string family = null;

        _mockRepository.Setup(x => x.GetUserInfoAsync(name, family))
                       .ReturnsAsync(userDTOs);

        _mockCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<List<UserInfo>>(), It.IsAny<TimeSpan>()))
                  .Returns(Task.CompletedTask);

        // Act
        var result = await _userProfileManagement.GetUserAsync(name, family);

        // Assert
        Assert.NotNull(result);

        foreach (var item in result)
            Assert.Equal("سارا", item.Name);

        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<List<UserInfo>>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsNull_WhenDbIsEmpty()
    {
        // Arrange
        CreatedNames(out string name, out string family);

        _mockCache.Setup(x => x.GetAsync<List<UserInfo>>(It.IsAny<string>()))
                  .ReturnsAsync((List<UserInfo>?)null);

        _mockRepository.Setup(x => x.GetUserInfoAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync(new List<UserInfoDTO>());

        // Act
        var result = await _userProfileManagement.GetUserAsync(name, family);

        // Assert
        Assert.Null(result);
    }

    private void CreatedNames(out string name, out string family)
    {
        var faker = new Faker("fa");
        name = faker.Name.FirstName();
        family = faker.Name.LastName();
    }
}

