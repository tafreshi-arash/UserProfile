using Microsoft.EntityFrameworkCore;


public class AppDbContext : DbContext
{
    public DbSet<UserInfoDTO> Users { get; set; }
    public DbSet<CertificateDTO> Certificates { get; set; }
    public DbSet<WorkHistoryDTO> WorkHistories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserInfoDTO>()
            .HasMany(c => c.Certificates)
            .WithOne(u => u.UserInfo)
            .HasForeignKey(u => u.UserId);


        builder.Entity<UserInfoDTO>()
            .HasMany(w => w.WorkHistories)
            .WithOne(u => u.UserInfo)
            .HasForeignKey(u => u.UserId);

        base.OnModelCreating(builder);
    }
}

