using ExamBot.Dal.Entities;
using ExamBot.Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace ExamBot.Dal;

public class MainContext : DbContext
{
    public DbSet<BotUser> botUsers { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Data Source=localhost\\SQLEXPRESS;User ID=sa;Password=1;Initial Catalog=ExamBot;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BotUserConfiguration());
        modelBuilder.ApplyConfiguration(new UserInfoConfiguration());

    }
}
