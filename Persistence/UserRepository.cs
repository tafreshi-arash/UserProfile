using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserInfoDTO>> GetUserInfoAsync(string? name, string? family)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return await _db.Users
                    .Where(x => x.Name.ToLower() == name.ToLower())
                    .Select(x => new UserInfoDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Family = x.Family,
                        Email = x.Email,
                        Certificates = x.Certificates.Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Institute = c.Institute,
                            IssuedTime = c.IssuedTime
                        }).ToList(),
                        WorkHistories = x.WorkHistories.Select(w => new WorkHistoryDTO
                        {
                            Id = w.Id,
                            CompanyName = w.CompanyName,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description
                        }).ToList()
                    })
                    .ToListAsync();
            }
            else if (!string.IsNullOrEmpty(family))
            {
                return await _db.Users
                    .Where(x => x.Family.ToLower() == family.ToLower())
                    .Select(x => new UserInfoDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Family = x.Family,
                        Email = x.Email,
                        Certificates = x.Certificates.Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Institute = c.Institute,
                            IssuedTime = c.IssuedTime
                        }).ToList(),
                        WorkHistories = x.WorkHistories.Select(w => new WorkHistoryDTO
                        {
                            Id = w.Id,
                            CompanyName = w.CompanyName,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description
                        }).ToList()
                    })
                    .ToListAsync();
            }
            else if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(family))
            {
                return await _db.Users
                    .Where(x => x.Name.ToLower() == name.ToLower() && x.Family.ToLower() == family.ToLower())
                    .Select(x => new UserInfoDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Family = x.Family,
                        Email = x.Email,
                        Certificates = x.Certificates.Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Institute = c.Institute,
                            IssuedTime = c.IssuedTime
                        }).ToList(),
                        WorkHistories = x.WorkHistories.Select(w => new WorkHistoryDTO
                        {
                            Id = w.Id,
                            CompanyName = w.CompanyName,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description
                        }).ToList()
                    })
                    .ToListAsync();
            }
            else
            {
                return await _db.Users
                    .Select(x => new UserInfoDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Family = x.Family,
                        Email = x.Email,
                        Certificates = x.Certificates.Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Institute = c.Institute,
                            IssuedTime = c.IssuedTime
                        }).ToList(),
                        WorkHistories = x.WorkHistories.Select(w => new WorkHistoryDTO
                        {
                            Id = w.Id,
                            CompanyName = w.CompanyName,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description
                        }).ToList()
                    })
                    .ToListAsync();
            }
        }

    }
}
